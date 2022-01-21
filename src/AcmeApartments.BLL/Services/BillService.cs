using AcmeApartments.BLL.DTOs;
using AcmeApartments.BLL.Interfaces;
using AcmeApartments.DAL.Identity;
using AcmeApartments.DAL.Interfaces;
using AcmeApartments.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AcmeApartments.BLL.Services
{
    public class BillService : IBillService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }

        public async Task<PaymentsViewModelDto> GetBills(AptUser user)
        {
            var waterBills = await _unitOfWork.WaterBillRepository.Get().ToListAsync();
            var electricBills = await _unitOfWork.ElectricBillRepository.Get().ToListAsync();
            var newWaterBill = new WaterBill();
            var newElectricBill = new ElectricBill();

            if (waterBills.Count == 0)
            {
                newWaterBill = new WaterBill
                {
                    User = user,
                    Amount = 42.53M,
                    DateDue = DateTime.Now.AddDays(20)
                };

                await _unitOfWork.WaterBillRepository.Insert(newWaterBill);
                await _unitOfWork.Save();
            }

            if (electricBills.Count == 0)
            {
                newElectricBill = new ElectricBill
                {
                    User = user,
                    Amount = 96.53M,
                    DateDue = DateTime.Now.AddDays(20)
                };

                await _unitOfWork.ElectricBillRepository.Insert(newElectricBill);
                await _unitOfWork.Save();
            }

            return new PaymentsViewModelDto
            {
                User = user,
                WaterBill = waterBills.Count == 0 ? newWaterBill : waterBills[0],
                ElectricBill = waterBills.Count == 0 ? newElectricBill : electricBills[0]
            };
        }
    }
}
