using Microsoft.IdentityModel.Tokens;
using NewsWebApi.IRepository;
using NewsWebApi.Repository;
using SharedLib.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewsWebApi.Services
{
    public class AccountService
    {
        public AccountService(IAdminRepository adminRepository, ITempPasswordRepository tempPasswordRepository, JwtService jwtService, EmailService emailService)
        {
            AdminRepository = adminRepository;
            TempPasswordRepository = tempPasswordRepository;
            JwtService = jwtService;
            EmailService = emailService;
        }

        public IAdminRepository AdminRepository { get; }
        public ITempPasswordRepository TempPasswordRepository { get; }
        public JwtService JwtService { get; }
        public EmailService EmailService { get; }

        public async Task<ResponseDTO<object>> SendPasswordToMail(string email)
        {
            var admin = await AdminRepository.GetAsync(email);
            if (admin != null)
            {
                var password = RandomString();
                await TempPasswordRepository.AddPassword(email, password);
                await EmailService.SendEmailAsync(admin.Email, $"مرحبا {admin.Name} هذه هي كلمه المرور", $"مرحبا {admin.Name} هذه هي كلمه المرور لا تشاركها مع احد صالحه لمده 5 دقايق \n {password}");
                return new ResponseDTO<object> { Success = true };
            }
            return new ResponseDTO<object> { Message = "User not found" };
        }


        public async Task<ResponseDTO<string>> Login(UserLoginDTO userLoginDTO)
        {
            var password = await TempPasswordRepository.GetPassword(userLoginDTO.Email);
            if (password == null)
            {
                return new ResponseDTO<string> { Message = "قم بارسال كلمه المرور عبر الايميل اولا" };
            }
            else
            {
                if (password.Date > DateTime.Now.AddHours(2))
                {
                    return new ResponseDTO<string> { Message = "كلمه المرور انتهت" };
                }
                else if (password.Password == userLoginDTO.Password)
                {
                    var admin = await AdminRepository.GetAsync(userLoginDTO.Email);
                    var roles = admin.UserRoles.Select(x => x.Role.Name);
                    var token = JwtService.GenerateToken(userLoginDTO.Email , roles);
                    return new ResponseDTO<string> { Success = true, Data = token };
                }
                else {
                    return new ResponseDTO<string> { Message="كلمه المرور غير صحيحه"};
                }
            }

        }
        public string RandomString(int size = 32)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(46 * random.NextDouble() + 48)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

    }
}
