using Microsoft.AspNetCore.Mvc.Rendering;
using MiniStore.Core.Interface;
using MiniStore.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MiniStore.Core.Services
{
    public class LocalGovernmentService : ILocalGovernmentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocalGovernmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<SelectListItem> GetLocalGovernmentDropdownList(string stateId)
        {
            var localGovernments = _unitOfWork.LocalGovernment.GetLocalGovernmentListForDropDown(stateId);

            return localGovernments;
        }
    }
}
