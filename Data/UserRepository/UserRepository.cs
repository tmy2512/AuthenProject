using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserRepository(IConfiguration configuration)
        {
            // Lấy chuỗi kết nối từ cấu hình
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _passwordHasher = new PasswordHasher<User>();
        }

     

        public async Task RegisterUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Hash mật khẩu trước khi lưu vào cơ sở dữ liệu
                var hashedPassword = _passwordHasher.HashPassword(user, user.MemberPassword);

                using (var command = new SqlCommand("AddUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MemberName", (object)user.MemberName ?? DBNull.Value);
                    command.Parameters.AddWithValue("@YearOfBirth", user.YearOfBirth);
                    command.Parameters.AddWithValue("@Email", (object)user.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Phone", (object)user.Phone ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Gender", (int)user.Gender); // Enum to int
                    command.Parameters.AddWithValue("@MemberPassword", (object)hashedPassword ?? DBNull.Value);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<LoginResult> ValidateUser(string email, string password)
        {
            using (var connection = new SqlConnection(_connectionString))

            {
                using (var command = new SqlCommand("ValidateUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Thêm tham số email vào câu lệnh
                    command.Parameters.AddWithValue("@Email", email);
                    //command.Parameters.AddWithValue("@Password", password);

                    // Mở kết nối và thực thi câu lệnh
                    await connection.OpenAsync();
                    // var result = await command.ExecuteScalarAsync();
                    var reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        // Fetch the email and member password from the reader
                        var storedEmail = reader.GetString(reader.GetOrdinal("Email"));
                        var hashedPassword = reader.GetString(reader.GetOrdinal("MemberPassword"));

                        // Check if the email matches and verify the password
                        if (email == storedEmail)
                        {
                            try
                            {
                                if (IsBase64String(hashedPassword))
                                {
                                    var verifyResult = _passwordHasher.VerifyHashedPassword(new User { MemberPassword = password }, hashedPassword, password);
                                    if (verifyResult == PasswordVerificationResult.Success)
                                    {
                                        return new LoginResult { Result = true, Message = "Login successful" };
                                    }
                                    else
                                    {
                                        return new LoginResult { Result = false, Message = "Incorrect password" };
                                    }
                                }
                                else
                                {
                                    return new LoginResult { Result = false, Message = "Invalid hashed password format" };
                                }

                            }
                            catch (Exception e)
                            {
                                return new LoginResult { Result = false, Message = $"An error occurred: {e.Message}" };
                            }

                            
                               
                        }

                    }

                    return new LoginResult { Result = false, Message = "Email not found" }; // Nếu không tìm thấy người dùng, trả về false
                }
            }
        }
        private bool IsBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                return false;

            base64 = base64.Trim();

            // Check length for padding characters
            if (base64.Length % 4 != 0)
                return false;

            // Check for invalid characters
            foreach (char c in base64)
            {
                if (!((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || (c >= '0' && c <= '9') || c == '+' || c == '/' || c == '='))
                    return false;
            }

            return true;
        }

    }
}
