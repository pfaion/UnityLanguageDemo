﻿/**
 * This is part of the NEEDSIM Life simulation plug in for Unity, version 1.0.2
 * Copyright 2014 - 2016 Fantasieleben UG (haftungsbeschraenkt)
 *
 * http;//www.fantasieleben.com for further details.
 *
 * For questions please get in touch with Tilman Geishauser: tilman@fantasieleben.com
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace NEEDSIM
{
    /// <summary>
    /// Participate in a slot to satisfy a goal.
    /// </summary>
    public class SatisfyGoalWithConversations : Action
    {
		public SatisfyGoalWithConversations(NEEDSIMNode agent)
            : base(agent)
        { }

        public override string Name
        {
            get
            {
                return "SatisfyGoal";
            }
        }

        /// <summary>
        /// Satisfying a goal at an AffordanceTree node.
        /// </summary>
        /// <returns>Success if need satsifaction goal was achieved, running whilst it is being satisfied. </returns>
        public override Action.Result Run()
        {
            if (agent.Blackboard.currentState == Blackboard.AgentState.ExitNEEDSIMBehaviors)
            {
                //Actions should be interrupted until the agent state is dealt with.
                return Result.Failure;
            }

            if (agent.Blackboard.activeSlot.SlotState == Simulation.Slot.SlotStates.Blocked)
            {
                return Result.Failure;
            }

            // Check whether the current interaction is still running. This is in a seperate block as it 
            // might be edited for some usage scenarios.
            bool interactionStillRunning = false;
            if (!agent.AffordanceTreeNode.Parent.Affordance.InteractionStartedThisFrame
                && agent.AffordanceTreeNode.Parent.Affordance.CurrentInteraction != null)
            {
                interactionStillRunning = true;
            }


			// CONVERSATION: check for conversation allowed, by requesting the ConversationController component
			ConversationController convObj = agent.AffordanceTreeNode.Parent.gameObject.GetComponent<ConversationController>();


            //If goal is achieved get ready for next action
            if (!interactionStillRunning
                && agent.AffordanceTreeNode.Goal.HasBeenAchieved)
            {
				


                //If you do not want agents to stay at the same slot until their type of goal is finished you can just go to the else case
                Simulation.Goal newGoal = agent.AffordanceTreeNode.SatisfactionLevels.GoalToSatisfyLowestNeed();
                if (newGoal.NeedToSatisfy == agent.AffordanceTreeNode.Goal.NeedToSatisfy)
                {
                    agent.AffordanceTreeNode.Goal = newGoal;


                }
                else
                {                 
                    agent.Blackboard.activeSlot.AgentDeparture();
                    agent.Blackboard.currentState = Blackboard.AgentState.PonderingNextAction;

					// CONVERSATION: try to deregister. if not deregistered yet, continue interaction
					if (convObj != null && ! convObj.DeregisterAgent (agent)) {
						return Result.Running;
					}


                    return Result.Success;
                }
            }

			// CONVERSATION: if not yet registered (just started interaction), register the agent
			if (convObj != null && !convObj.AgentRegistered(agent)) {
				convObj.RegisterAgent (agent);
			}


            //Participate in current interaction or start a new one
            if (agent.AffordanceTreeNode.Parent.Affordance.CurrentInteraction != null)
            {
                agent.AffordanceTreeNode.ApplyParentInteraction();
				agent.Blackboard.currentState = Blackboard.AgentState.ParticipatingSlot;
            }
            else
            {
                agent.AffordanceTreeNode.Parent.Affordance.StartRandomInteraction();
            }


            return Result.Running;
        }
    }
}