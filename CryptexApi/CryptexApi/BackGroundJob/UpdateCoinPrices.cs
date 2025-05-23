using CryptexApi.UnitOfWork;
using Quartz;

namespace CryptexApi.BackGroundJob
{
    public class UpdateCoinPrices : IJob
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCoinPrices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _unitOfWork.CoinRepository.UpdatePricesFromBinance();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
