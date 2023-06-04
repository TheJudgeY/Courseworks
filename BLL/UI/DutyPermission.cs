using Core.Enums;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.ConsoleManagers;

namespace UI
{
    public class DutyPermission
    {
        private readonly StateManagerUI _stateManagerUI;
        private readonly TesterUI _testerUI;
        private readonly DeveloperUI _developerUI;
        public DutyPermission(StateManagerUI stateManagerUI, TesterUI testerUI, DeveloperUI developerUI)
        {
            _stateManagerUI = stateManagerUI;
            _testerUI = testerUI;
            _developerUI = developerUI;
        }
        public async Task DutyIdentifier(UserProjectRole table)
        {
            switch (table.Duty)
            {
                case Duty.StateManager:
                    await _stateManagerUI.PerformOperationsAsync(table.UserId, table.ProjectId);
                    break;
                case Duty.Tester:
                    await _testerUI.PerformOperationsAsync(table.ProjectId, table.UserId);
                    break;
                case Duty.Developer:
                    await _developerUI.PerformOperationsAsync(table.ProjectId, table.UserId);
                    break;
                default:
                    throw new Exception("Wrong data input");
            }
        }
    }
}
