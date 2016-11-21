using System.Collections.Generic;
using Analyzer.StateMachine.States;
using Analyzer.StateMachine.Rules;
using System;

namespace Analyzer.StateMachine
{
	internal class StateMachine
	{
		public IState CurrentState { get; private set; }
		public Dictionary<string, IState> States { get; private set; }
		public List<ITransitionRule> Rules { get; private set; }

		public StateMachine()
		{
			InitStates();
			InitRules();
		}

		private void InitStates()
		{
			States = new Dictionary<string, IState>();

			var idleState = new IdleState();
			var bracketOpenState = new BraceOpenState();
			var bracketCloseState = new BraceCloseState();
			var operandState = new OperandState();
			var binaryOperatorState = new BinaryOperatorState();
			var unaryOperatorState = new UnaryOperatorState();
			var finalState = new FinalState();

			States.Add(nameof(IdleState), idleState);
			States.Add(nameof(BraceOpenState), bracketOpenState);
			States.Add(nameof(BraceCloseState), bracketCloseState);
			States.Add(nameof(OperandState), operandState);
			States.Add(nameof(BinaryOperatorState), binaryOperatorState);
			States.Add(nameof(UnaryOperatorState), unaryOperatorState);
			States.Add(nameof(FinalState), finalState);

			CurrentState = idleState;
		}

		private void InitRules()
		{
			Rules = new List<ITransitionRule>();

			var idleState = States[nameof(IdleState)];
			var operandState = States[nameof(OperandState)];
			var binaryOperatorState = States[nameof(BinaryOperatorState)];
			var unaryOperatorState = States[nameof(UnaryOperatorState)];
			var finalState = States[nameof(FinalState)];
			var bracketOpenState = States[nameof(BraceOpenState)];
			var bracketCloseState = States[nameof(BraceCloseState)];

			var idleToOperandRule = new OperandRule(idleState, operandState, 1);
			var idleToUnaryOpRule = new UnaryOperatorRule(idleState, unaryOperatorState, 2);
			var operandToBinaryRule = new BinaryOperatorRule(operandState, binaryOperatorState, 2);
			var binaryToOperandRule = new OperandRule(binaryOperatorState, operandState, 2);
			var unaryToOperandRule = new OperandRule(unaryOperatorState, operandState, 2);
			var operandToFinishRule = new FinishRule(operandState, finalState, 0);
			var idleToBracketOpenerRule = new BraceOpenRule(idleState, bracketOpenState, 5);
			var unaryToBracketOpenerRule = new BraceOpenRule(unaryOperatorState, bracketOpenState, 5);
			var bracketOpenToOperandRule = new OperandRule(bracketOpenState, operandState, 4);
			var operandToBracketCloserRule = new BraceCloseRule(operandState, bracketCloseState, 5);
			var bracketOpenerToCloserRule = new BraceCloseRule(bracketOpenState, bracketCloseState, 5);
			var bracketCloserToFinishRule = new FinishRule(bracketCloseState, finalState, 1);
			var bracketCloserToBinaryOpRule = new BinaryOperatorRule(bracketCloseState, binaryOperatorState, 2);
			var binaryToUnaryOpRule = new UnaryOperatorRule(binaryOperatorState, unaryOperatorState, 3);

			Rules.Add(idleToOperandRule);
			Rules.Add(idleToUnaryOpRule);
			Rules.Add(operandToBinaryRule);
			Rules.Add(binaryToOperandRule);
			Rules.Add(unaryToOperandRule);
			Rules.Add(operandToFinishRule);
			Rules.Add(idleToBracketOpenerRule);
			Rules.Add(unaryToBracketOpenerRule);
			Rules.Add(operandToBracketCloserRule);
			Rules.Add(bracketOpenerToCloserRule);
			Rules.Add(bracketCloserToFinishRule);
			Rules.Add(bracketOpenToOperandRule);
			Rules.Add(binaryToUnaryOpRule);
			Rules.Add(bracketCloserToBinaryOpRule);
		}

		public void UpdateState(string item)
		{
			List<ITransitionRule> validRules = new List<ITransitionRule>();
			bool hasAnyRulesForThisState = false;

			foreach (var rule in Rules)
			{
				if (rule.SourceState == CurrentState)
				{
					hasAnyRulesForThisState = true;

					if (rule.CanTransit(item))
					{
						validRules.Add(rule);
					}
				}
			}

			if (!hasAnyRulesForThisState)
			{
				throw new StateMachineException("No transitions from this state.");
			}

			else if (validRules.Count > 0)
			{
				var pickedRule = PickRuleWithHighestPriority(validRules);
				SwitchState(pickedRule.DestState, item);
			}
			else
			{
				throw new StateMachineException("No valid state to transit to.");
			}
		}

		private void SwitchState(IState state, string item)
		{
			state.OnStateEnter(CurrentState, item);

			CurrentState = state;
		}

		private ITransitionRule PickRuleWithHighestPriority(List<ITransitionRule> rules)
		{
			int maxPriority = 0;
			for (int i = 1; i < rules.Count; i++)
			{
				if (rules[i].Priority > rules[maxPriority].Priority)
				{
					maxPriority = i;
				}
			}

			return rules[maxPriority];
		}
	}
}
