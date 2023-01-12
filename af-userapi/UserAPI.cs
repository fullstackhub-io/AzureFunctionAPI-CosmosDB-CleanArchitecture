namespace af_userapi
{
    public class UserAPI
    {
        private readonly IMediator mediator;

        public UserAPI(IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = httpContextAccessor.HttpContext.RequestServices.GetService<IMediator>();
        }

        [FunctionName("Get")]
        public async Task<ActionResult<UserDTO>> Get(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Get/{email}")] HttpRequest req,
            ILogger log, string email)
        {
            return await this.mediator.Send(new GetSingleUserQuery { Email = email });
        }

        [FunctionName("GetAll")]
        public async Task<ActionResult<UserVM>> GetAllUser(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetAll/{userStatus}")] HttpRequest req,
            ILogger log, int userStatus)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            var status = (UserStatus)userStatus;
            return await this.mediator.Send(new GetAllUserQuery { Status = status });
        }

        [FunctionName("Add")]
        public async Task<ActionResult<string>> AddUser(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<AddUserCommand>(requestData);
            return await this.mediator.Send(user);
        }

        [FunctionName("Update")]
        public async Task<ActionResult<bool>> UpdateUser(
          [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
          ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<UpdateUserCommand>(requestData);
            return await this.mediator.Send(user);
        }

        [FunctionName("UpdateStatus")]
        public async Task<ActionResult<bool>> UpdateUserStatus(
          [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)] HttpRequest req,
          ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var user = JsonConvert.DeserializeObject<UpdateUserStatusCommand>(requestData);
            return await this.mediator.Send(user);
        }
    }
}
