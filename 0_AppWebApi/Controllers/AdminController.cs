using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

using Services;
using Configuration;
using Configuration.Options;

using Microsoft.Extensions.Options;
using Models.DTO;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AppWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]   
    public class AdminController : Controller
    {
        private readonly Encryptions _encryptions = null;
        private readonly DatabaseConnections _dbConnections = null;
        private readonly ILogger<AdminController> _logger;
        private readonly DbConnectionSetsOptions _dbSetOptions;
        private readonly AesEncryptionOptions _aesOptions;
        private readonly JwtOptions _jwtOptions;
        private readonly VersionOptions _versionOptions;
        private readonly IConfiguration _configuration;
        private readonly IAdminService _service;

        //GET: api/admin/environment
        [HttpGet()]
        [ActionName("Environment")]
        [ProducesResponseType(200, Type = typeof(DatabaseConnections.SetupInformation))]
        public IActionResult Environment()
        {
            try
            {
                var info = _dbConnections.SetupInfo;

                _logger.LogInformation($"{nameof(Environment)}:\n{JsonConvert.SerializeObject(info)}");
                return Ok(info);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Environment)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //GET: api/admin/version
        [HttpGet()]
        [ActionName("Version")]
        [ProducesResponseType(typeof(VersionOptions), 200)]
        public IActionResult Version()
        {
            try
            {
                _logger.LogInformation($"{nameof(Version)}");
                return Ok(_versionOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving version information");
                return BadRequest(ex.Message);
            }
        }

        //GET: api/admin/seed?nrItems={nrItems}
        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Seed(int nrItems = 4)
        {
            try
            {
                _logger.LogInformation($"{nameof(Seed)}");
                await _service.SeedAsync(nrItems);

                return Ok($"Successfully seeded {Math.Max(nrItems, 4)} countries into the database.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(Seed)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        //GET: api/admin/log
        [HttpGet()]
        [ActionName("Log")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LogMessage>))]
        public async Task<IActionResult> Log([FromServices] ILoggerProvider _loggerProvider)
        {
            //Note the way to get the LoggerProvider, not the logger from Services via DI
            if (_loggerProvider is InMemoryLoggerProvider cl)
            {
                return Ok(await cl.MessagesAsync);
            }
            return Ok("No messages in log");
        }

        public AdminController(Encryptions encryptions, 
                    DatabaseConnections dbConnections, 
                    ILogger<AdminController> logger,
                    IConfiguration configuration,
                    IOptions<DbConnectionSetsOptions> dbSetOptions,
                    IOptions<AesEncryptionOptions> aesOptions,
                    IOptions<JwtOptions> jwtOptions,
                    IOptions<VersionOptions> versionOptions,
                    IAdminService service)
        {
            _logger = logger;

            _dbSetOptions = dbSetOptions.Value;
            _aesOptions = aesOptions.Value;
            _jwtOptions = jwtOptions.Value;
            _versionOptions = versionOptions.Value;
            _configuration = configuration;

            _encryptions = encryptions;
            _dbConnections = dbConnections;

            _service = service;
        }

        //GET: api/admin/databaseoverview
        [HttpGet()]
        [ActionName("DatabaseOverview")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<DatabaseOverviewDto>))]
        public async Task<ActionResult<ResponseItemDto<DatabaseOverviewDto>>> DatabaseOverview()
        {
            try
            {
                var result = await _service.GetDatabaseOverviewAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching database overview view");
                return BadRequest(ex.Message);
            }
        }

        //GET: api/admin/removeseed
        [HttpGet()]
        [ActionName("RemoveSeed")]
        [ProducesResponseType(200, Type = typeof(ResponseItemDto<DatabaseOverviewDto>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RemoveSeed(string seeded = "true")
        {
            try
            {
                bool seededArg = bool.Parse(seeded);

                _logger.LogInformation($"{nameof(RemoveSeed)}: {nameof(seededArg)}: {seededArg}");
                var info = await _service.RemoveSeedAsync(seededArg);
                return Ok(info);        
            }
            catch (Exception ex)
            {
                _logger.LogError($"{nameof(RemoveSeed)}: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }


    }
}


