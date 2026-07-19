using Microsoft.AspNetCore.Mvc;
using IrinCibiBlazesoftChallenge.Models;
using IrinCibiBlazesoftChallenge.Services;

    namespace IrinCibiBlazesoftChallenge.Controllers
    {
        [ApiController]
        [Route("api/[controller]")]
        public class SlotController : ControllerBase
        {
            private readonly MongoService _mongoService;

            public SlotController(MongoService mongoService)
            {
                _mongoService = mongoService;
            }

            //Helper to register a player in the Swagger
            [HttpPost("player")]
            public async Task<IActionResult> CreatePlayer([FromBody] Players player)
            {
            if (string.IsNullOrWhiteSpace(player.Username))
            {
                return BadRequest("Username is required.");
            }

            if (player.Balance < 0)
            {
                return BadRequest("Balance cannot be negative.");
            }
            var created = await _mongoService.CreatePlayersAsync(player);
                return Ok(created);
            }

        //Executes a slot machine spin and returns the result, winnings, and updated balance.
        [HttpPost("spin")]
            public async Task<IActionResult> Spin([FromBody] SpinRequest request)
            {
                if (request == null || string.IsNullOrEmpty(request.PlayerId) || request.BetAmount <= 0)
                {
                    return BadRequest("Invalid request values.");
                }

                var config = await _mongoService.GetGameConfigAsync();

                //Deduct bet atomically
                var updatedPlayer = await _mongoService.TryDeductBalanceAsync(request.PlayerId, request.BetAmount);
                if (updatedPlayer == null)
                {
                    return BadRequest("Insufficient balance or player not found.");
                }

                //Spin Matrix
                int[][] matrix = SlotMachineEngine.GenerateMatrix(config.Width, config.Height);

                //Fetch win paths and scan them
                var paths = SlotMachineEngine.GetWinLinePaths(config.Width, config.Height);
                decimal totalWinMultiplier = 0;
                var details = new List<string>();

                foreach (var path in paths)
                {
                    int[] pathValues = path.Select(p => matrix[p.row][p.col]).ToArray();
                    int pathWinMultiplier = SlotMachineEngine.CalculatePathWin(pathValues);

                    if (pathWinMultiplier > 0)
                    {
                        totalWinMultiplier += pathWinMultiplier;
                        details.Add($"Won {pathWinMultiplier}x on path: {string.Join(",", pathValues)}");
                    }
                }

                decimal totalWinAmount = totalWinMultiplier * request.BetAmount;

                //Atomically add winnings back to player balance
                if (totalWinAmount > 0)
                {
                    updatedPlayer = await _mongoService.AdjustBalanceAsync(request.PlayerId, totalWinAmount);
                }

                return Ok(new SpinResponse
                {
                    Matrix = matrix,
                    WinAmount = totalWinAmount,
                    UpdatedBalance = updatedPlayer.Balance,
                    Details = details
                });
            }

        // Adds funds to the player's balance.
        [HttpPost("balance")]
            public async Task<IActionResult> UpdateBalance([FromBody] UpdateBalanceRequest request)
            {
                if (request == null || string.IsNullOrEmpty(request.PlayerId))
                {
                    return BadRequest("Invalid request.");
                }

                if (request.Amount <= 0)
                {
                return BadRequest("Amount must be greater than zero.");
                }

            var player = await _mongoService.AdjustBalanceAsync(request.PlayerId, request.Amount);
                if (player == null)
                {
                    return NotFound("Player not found.");
                }

                return Ok(new { message = "Balance updated.", balance = player.Balance });
            }

        // Updates the slot machine dimensions stored in MongoDB.
        [HttpPost("config")]
            public async Task<IActionResult> UpdateConfig([FromBody] UpdateConfigRequest request)
            {
                if (request.Width < 3 || request.Height < 3)
                {
                    return BadRequest("Matrix must be at least 3x3.");
                }

                await _mongoService.UpdateGameConfigAsync(request.Width, request.Height);
                return Ok(new { message = "Matrix dimensions re-configured without restarts!" });
            }
        }

        public class SpinRequest
        {
            public string PlayerId { get; set; } = null!;
            public decimal BetAmount { get; set; }
        }

        public class SpinResponse
        {
            public int[][] Matrix { get; set; } = null!;
            public decimal WinAmount { get; set; }
            public decimal UpdatedBalance { get; set; }
            public List<string> Details { get; set; } = new();
        }

        public class UpdateBalanceRequest
        {
            public string PlayerId { get; set; } = null!;
            public decimal Amount { get; set; }
        }

        public class UpdateConfigRequest
        {
            public int Width { get; set; }
            public int Height { get; set; }
        }
    }
