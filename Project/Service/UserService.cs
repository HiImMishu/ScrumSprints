using Project.Models;
using Project.Models.DTO;
using Project.Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Service
{
    public class UserService
    {
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;
        private readonly ITeamRepository teamRepository;
        private readonly TokenService tokenService;

        public UserService(UserRepository userRepository, ProductRepository productRepository, TeamRepository teamRepository, TokenService tokenService)
        {
            this.userRepository = userRepository;
            this.productRepository = productRepository;
            this.teamRepository = teamRepository;
            this.tokenService = tokenService;
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var user = await userRepository.GetUserByIdAsync(userId);
            var products = await productRepository.GetProductByUserIdAsync(userId);
            var teamsLeaded = await teamRepository.GetLeadedTeamsByUserId(userId);
            var teamsParticipated = await teamRepository.GetParticipatedTeamsByUserId(userId);

            if (user == null)
            {
                return null;
            }

            return new UserDTO()
            {
                Id = user.id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                SignedAt = user.SignedAt,
                ArchivedAt = user.ArchivedAt,
                Products = products.Select(p => new SimpleProductDTO() { Id = p.Id, Name = p.Name }).ToList(),
                TeamsLeaded = teamsLeaded.Select(t => new SimpleTeamDTO() { Id = t.Id, CreatedAt = t.CreatedAt, Name = t.Name}).ToList(),
                TeamParticipated = teamsParticipated.Select(t => new SimpleTeamDTO() { Id = t.Id, CreatedAt = t.CreatedAt, Name = t.Name }).ToList()
            };
        }
        
        public async Task<UserDTO> SaveUser(UserRegisterDTO userRegisterDTO)
        {
            if (await userRepository.GetUserByEmail(userRegisterDTO.Email) != null)
                return null;

            CreatePasswordHash(userRegisterDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User()
            {
                id = 0,
                FirstName = userRegisterDTO.FirstName,
                LastName = userRegisterDTO.LastName,
                Email = userRegisterDTO.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            await userRepository.SaveUser(user);
            return await GetUserByIdAsync(user.id);
        }

        public async Task<bool> UpdateUser(SimpleUserDTO user)
        {
            return await userRepository.UpdateUser(user);
        }

        public void ArchiveUser(int userId)
        {
            userRepository.ArchiveUser(userId);
        }

        public async Task<TokenDTO> AuthenticateUser(UserCredentials userCredentials)
        {
            var user = await userRepository.GetUserByEmail(userCredentials.Email);
            if (user == null)
                return null;

            if (VerifyPasswordHash(userCredentials.Password, user.PasswordHash, user.PasswordSalt))
                return tokenService.GetToken(user);
            else
                throw new ArgumentException("Invalid password.");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                var str = System.Text.Encoding.UTF8.GetString(hmac.Key);
                var salt = System.Text.Encoding.UTF8.GetBytes(str);
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
