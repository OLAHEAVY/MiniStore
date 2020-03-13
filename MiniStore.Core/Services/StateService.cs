using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Core.Interface;
using MiniStore.Data.Entities;
using MiniStore.Data.Repository.IRepository;
using MiniStore.Utility.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Core.Services
{
    public class StateService : IStateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILog _logger;
        public StateService(IUnitOfWork unitOfWork, ILog logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<SelectListItem> GetAllStatesDropDown()
        {
            var states = _unitOfWork.State.GetStateListForDropDown();

            return states;
        }
    }
}
