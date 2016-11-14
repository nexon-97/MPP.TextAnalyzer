using System.Collections.Generic;
using Analyzer.StateMachine.States;
using Analyzer.StateMachine.Rules;

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
			var operandState = new OperandState();
			var binaryOperatorState = new BinaryOperatorState();
			var unaryOperatorState = new UnaryOperatorState();
			var finalState = new FinalState();

			States.Add(nameof(IdleState), idleState);
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

			var idleToOperandRule = new OperandRule(idleState, operandState, 1);
			var idleToUnaryOpRule = new UnaryOperatorRule(idleState, unaryOperatorState, 2);
			var operandToBinaryRule = new BinaryOperatorRule(operandState, binaryOperatorState, 2);
			var binaryToOperandRule = new OperandRule(binaryOperatorState, operandState, 2);
			var unaryToOperandRule = new OperandRule(unaryOperatorState, operandState, 2);
			var operandToFinishRule = new FinishRule(operandState, finalState, 0);

			Rules.Add(idleToOperandRule);
			Rules.Add(idleToUnaryOpRule);
			Rules.Add(operandToBinaryRule);
			Rules.Add(binaryToOperandRule);
			Rules.Add(unaryToOperandRule);
			Rules.Add(operandToFinishRule);
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
				SwitchState(pickedRule.DestState);
			}
			else
			{
				throw new StateMachineException("No valid state to transit to.");
			}
		}

		private void SwitchState(IState state)
		{
			state.OnStateEnter(CurrentState);

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
