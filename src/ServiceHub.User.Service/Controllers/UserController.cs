using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using ServiceHub.User.Context.Repositories;
using ServiceHub.User.Context.Utilities;

namespace ServiceHub.User.Service.Controllers
{
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository,
                              ILoggerFactory loggerFactory)
          : base(loggerFactory)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns>OkObjectResult with an IEnumerable of all users,
        /// or a 500 StatusCodeResult if an error occurs.</returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var contextUsers = await _userRepository.GetAsync();
                var libraryUsers = UserModelMapper.List_ContextToLibrary(contextUsers);
                if (libraryUsers == null)
                {
                    logger.LogError("One or more users in the db were invalid.");
                    return new StatusCodeResult(500);
                }
                return Ok(libraryUsers);
            }
            catch(Exception e)
            {
                logger.LogError(e, "Status Code result of 500, failed to get users");
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Finds the user based on the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>the user with matching Id, or a 404 error</returns>
        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(200, Type = typeof(Library.Models.User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Get(Guid id)
        {
            try
            {
                var libraryUser = UserModelMapper.ContextToLibrary(await _userRepository.GetByIdAsync(id));
                if (libraryUser == null)
                {
                    logger.LogError("Library user model was null.");
                    return NotFound();
                }
                else
                {
                    return Ok(libraryUser);
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "Resource was not found in database.");
                return NotFound(e);
            }
        }

        /// <summary>
        /// Finds the users by Gender.
        /// </summary>
        /// <param name="gender"></param>
        /// <returns>all users with the specified gender, or a 400 error
        /// if the gender isn't valid, or a 500 error if a database error
        /// occured.</returns>
        [HttpGet("gender/{gender}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByGender(string gender)
        {
            try
            {
                string[] genders = ServiceHub.User.Library.Models.User.ValidUppercaseGenders;
                string upperGender = gender.ToUpper();
                bool validGender = false;

                foreach (var x in genders)
                {
                    if (upperGender == x)
                    {
                        validGender = true;
                    }
                }

                if (!validGender)
                {
                    logger.LogError($"Invalid Gender was input, {gender}.  ");
                    return BadRequest($"Invalid gender: {gender}.");
                }
                else
                {
                    var users = await _userRepository.GetAsync();
                    var GUsers = new List<ServiceHub.User.Library.Models.User>();

                    foreach (var x in users)
                    {
                        if (x.Gender.ToUpper() == upperGender)
                        {
                            var libraryUser = UserModelMapper.ContextToLibrary(x);
                            if (libraryUser == null)
                            {
                                logger.LogError("Library User model was null.");
                                return new StatusCodeResult(500);
                            }
                            GUsers.Add(libraryUser);
                        }
                    }
                    return Ok(GUsers);
                }
            }
            catch(Exception e)
            {
                logger.LogError(e, "GetByGender threw exception");
                return new StatusCodeResult(500);
            }
        }


        /// <summary>
        /// Gets all users of a certain type.
        /// </summary>
        /// <param name="type"> A string representing the type of user to filter by. </param>
        /// <returns>If the type is not an accepted type, returns a 400 StatusCodeResult. If
        /// the server fails to return a complete list of valid users, returns a 500
        /// StatusCodeResut. Otherwise returns a list of validated Users. </returns>
        [HttpPost]
        [Route("type")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Library.Models.User>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetByType([FromBody] string type)
        {
            try
            {
                if (type == null)
                {
                    logger.LogError("Null Type was input");
                    return BadRequest("Invalid type.");
                }
                bool isValidType = false;
                foreach (var validType in Library.Models.User.ValidUppercaseTypes)
                {
                    if (type.ToUpper() == validType) { isValidType = true; }
                }
                if (!isValidType)
                {
                    logger.LogError("Invalid Type was input");
                    return BadRequest("Invalid type.");
                }

                var users = await _userRepository.GetAsync();
                var contextUsers = new List<Context.Models.User>();
                foreach (var contextUser in users)
                {
                    if (contextUser.Type.ToUpper() == type.ToUpper())
                    {
                        contextUsers.Add(contextUser);
                    }
                }
                var libraryUsers = UserModelMapper.List_ContextToLibrary(contextUsers);
                return Ok(libraryUsers);
            }
            catch(Exception e)
            {
                logger.LogError(e, "Failed to access users by Type");
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Updates the user's address and/or location
        /// </summary>
        /// <param name="user">the user to update</param>
        /// <returns>204 NoContent if the update is successful, 400 Bad Request
        /// if the user id, location or address are invalid, or 500
        /// Internal Server Error if a database error occurs.</returns>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Put([FromBody]ServiceHub.User.Library.Models.User user)
        {
            try
            {
                if (user == null)
                {
                    logger.LogError("User model was null");
                    return BadRequest("Invalid user: object was null");
                }
                else
                {
                    var id = user.UserId;

                    if (user.UserId == Guid.Empty)
                    {
                        logger.LogError("User ID does not exist.");
                        return BadRequest("Invalid User Id");
                    }
                    var contextUser = await _userRepository.GetByIdAsync(user.UserId);
                    if (contextUser == null)
                    {
                        logger.LogError("User does not exist");
                        return BadRequest("User not found");
                    }

                    var libraryUser = UserModelMapper.ContextToLibrary(contextUser);
                    if (libraryUser == null)
                    {
                        logger.LogError("Library User model is null.");
                        return new StatusCodeResult(500);
                    }

                    if (user.Location != null) { libraryUser.Location = user.Location; }
                    libraryUser.Address = user.Address;
                    contextUser = UserModelMapper.LibraryToContext(libraryUser);

                    if (contextUser == null)
                    {
                        logger.LogError("Invalid update of location or address.");
                        return BadRequest("Invalid update of location or address.");
                    }
                    await _userRepository.UpdateAsync(contextUser);
                    return NoContent();

                }
            }
            catch(Exception e)
            {
                logger.LogError(e, "Exception thrown during Put");
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user"> A User model to be provided from an external source
        /// via JSON.  If the model is a valid model, it will be cast to a db-ready model
        /// and stored in the database. </param>
        /// <returns> If the user is accepted, it will return a 201, Created code.
        /// Otherwise, it will return a 400, client-error code. </returns>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Post([FromBody] User.Library.Models.User user)
        {
            try
            {
                if (user == null)
                {
                    logger.LogError("User is null.");
                    return BadRequest("Invalid user: User is null");
                }
                user.UserId = Guid.NewGuid();
                var contextUser = UserModelMapper.LibraryToContext(user);

                if (contextUser == null)
                {
                    logger.LogError("Context User is null.");
                    return BadRequest("Invalid user: Validation failed");
                }
                await _userRepository.InsertAsync(contextUser);

                return CreatedAtRoute("GetById", new { id = user.UserId }, user);
            }
            catch(Exception e)
            {
                logger.LogError(e, "Exception thrown during Post");
                return new StatusCodeResult(500);
            }
        }
    }
}
