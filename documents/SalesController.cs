namespace WebDemo.Controller.src
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly IOrderDetailService _orderDetailService;

        public SaleController(IOrderDetailService orderDetailService)
        {
            _orderDetailService = orderDetailService;
        }

        [HttpGet("most-purchased-product-lines")]
        public async Task<IActionResult> GetMostPurchasedProductLines([FromQuery] MostPurchasedQuery query)
        {
            var productLines = await _orderDetailService.GetMostPurchasedProductLinesAsync(query);
            return Ok(productLines);
        }

        [HttpGet("most-purchased-categories")]
        public async Task<IActionResult> GetMostPurchasedCategories([FromQuery] MostPurchasedQuery query)
        {
            var categories = await _orderDetailService.GetMostPurchasedCategoriesAsync(query);
            return Ok(categories);
        }

        [HttpGet("total-sales-last-12-months")]
        public async Task<IActionResult> GetTotalSalesLast12Months()
        {
            var sales = await _orderDetailService.GetTotalSalesLast12MonthsAsync();
            return Ok(sales);
        }

        [HttpGet("average-daily-sales")]
        public async Task<IActionResult> GetAverageDailySales([FromQuery] AverageDailySalesQuery query)
        {
            var averageSales = await _orderDetailService.GetAverageDailySalesAsync(query);
            return Ok(averageSales);
        }

        [HttpGet("new-customers-stats")]
        public async Task<IActionResult> GetNewCustomersStats()
        {
            var stats = await _orderDetailService.GetNewCustomersStatsAsync();
            return Ok(stats);
        }
    }
}