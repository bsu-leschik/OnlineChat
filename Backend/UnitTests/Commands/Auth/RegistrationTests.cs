// using BusinessLogic.Commands.Auth.Registration;
// using Database;
// using Entities;
// using Microsoft.AspNetCore.Identity;
// using Moq;
//
// namespace UnitTests.Commands.Auth;
//
// public class RegistrationTests
// {
//     private readonly Mock<IStorageService> _storageMock = new();
//
//     [Fact]
//     public async void Registration_CreatesUser()
//     {
//         _storageMock.Setup(s => s.GetUserByUsername(It.IsAny<string>(), Any<CancellationToken>(), Any<IQueryOptions<User>>()))
//                     .Returns(Task.FromResult<User>(null));
//         var handler = new RegistrationRequestHandler(_storageMock.Object, new PasswordHasherMock());
//         var request = new RegistrationCommand { Username = "123", Password = "232" };
//         var result = await handler.Handle(request, CancellationToken.None);
//         _storageMock.Verify(s => s.AddUserAsync(Any<User>(), Any<CancellationToken>()), Times.Once);
//         Assert.Equal(RegistrationResponse.Success, result);
//     }
//
//     [Fact]
//     public async void Registration_ErrorWithDuplicateUsername()
//     {
//         const string username = "username";
//         const string password = "password";
//
//         _storageMock.Setup(s => s.GetUserByUsername(It.IsAny<string>(), Any<CancellationToken>(), Any<IQueryOptions<User>>()))
//                     .Returns(Task.FromResult<User>(new User(username, password))!);
//         var handler = new RegistrationRequestHandler(_storageMock.Object, new PasswordHasherMock());
//         var request = new RegistrationCommand
//                           {
//                               Username = username, 
//                               Password = "123"
//                           };
//         var result = await handler.Handle(request, CancellationToken.None);
//         _storageMock.Verify(s => s.AddUserAsync(Any<User>(), Any<CancellationToken>()), Times.Never);
//         Assert.Equal(RegistrationResponse.DuplicateUsername, result);
//     }
//
//     private static T Any<T>()
//     {
//         return It.IsAny<T>();
//     }
//
//     private class PasswordHasherMock : IPasswordHasher<User>
//     {
//         public string HashPassword(User user, string password)
//         {
//             return password;
//         }
//
//         public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
//         {
//             return hashedPassword == providedPassword
//                 ? PasswordVerificationResult.Success
//                 : PasswordVerificationResult.Failed;
//         }
//     }
// }