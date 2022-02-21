using System.Text;
using System.Security.Cryptography;

// Password is any UTF-16 string
string clearPassword = "My$upeR Secr37 _123✨";
byte[] clearPasswordBytes = Encoding.Unicode.GetBytes(clearPassword);

Console.WriteLine($"-- Password ---");
Console.WriteLine($"Clear...: {clearPassword}");
Console.WriteLine($"(Hex)...: {Convert.ToHexString(clearPasswordBytes)}");
Console.WriteLine($"(Base64): {Convert.ToBase64String(clearPasswordBytes)}");
Console.WriteLine();

// Configuration

// Hash: 512 random bits (32 bytes), any other lenght is ok
int hashSizeInBytes = 512 / 8;
// Salt: 128 random bits (16 bytes), any other lenght is ok
int saltSizeInBytes = 128 / 8;
// Hash will use NIST recommended PBKDF2 config: https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#pbkdf2
// Rfc2898DeriveBytes default: 1k SHA1 iterations
// Recommended + random number of iterations (not a recommendation, but add some chaos)
int iterations = 310000 + RandomNumberGenerator.GetInt32(0, 10000);
var hashAlgo = HashAlgorithmName.SHA256;

// Use this to create you own salt
// byte[] salt = RandomNumberGenerator.GetBytes(saltSizeInBytes);
// var hashed = new Rfc2898DeriveBytes(clearPassword, salt, iterations, hashAlgo);

// Create a hash with salt size, number of iterations and a HMAC algo
var hashGenerator = new Rfc2898DeriveBytes(clearPassword, saltSizeInBytes, iterations, hashAlgo);

// The salt
byte[] saltBytes = hashGenerator.Salt;
// The hash
byte[] hashBytes = hashGenerator.GetBytes(hashSizeInBytes);
// The HMAC algo
string algo = hashGenerator.HashAlgorithm.Name ?? "";
// The iteration count
int iterationCount = hashGenerator.IterationCount;

// The value to persist
string passwordToPersist = $"{Convert.ToBase64String(saltBytes)}|{iterationCount}|{Convert.ToBase64String(hashBytes)}";

Console.WriteLine($"-- Rfc2898DeriveBytes ---");
Console.WriteLine($"Random Salt (Hex).....: {Convert.ToHexString(saltBytes)}");
Console.WriteLine($"Random Salt (Base64)..: {Convert.ToBase64String(saltBytes)}");

Console.WriteLine($"\nHashing Algorithm.....: {algo} ({iterationCount} iterations)");

Console.WriteLine($"Hash (Hex)............: {Convert.ToHexString(hashBytes)}");
Console.WriteLine($"Hash (Base64).........: {Convert.ToBase64String(hashBytes)}");

Console.WriteLine($"\n-- Persistent password string --\n{passwordToPersist}");

// Testing password

/// Query the persistance and parse parts
string[] storedHashParts = passwordToPersist.Split("|");
byte[] storedSalt = Convert.FromBase64String(storedHashParts[0]);
int storedIterationCount = Convert.ToInt32(storedHashParts[1]);
string storedPasswordHash = storedHashParts[2];

/// Generate hash to compare -- Incorrect case
string typedPassword = "bad$robot";
var testHashGenerator = new Rfc2898DeriveBytes(typedPassword, storedSalt, storedIterationCount, hashAlgo);
byte[] testHashBytes = testHashGenerator.GetBytes(hashSizeInBytes);
string testHash = Convert.ToBase64String(testHashBytes);

Console.WriteLine($"\n-- Incorrect attempt to log in --");
Console.WriteLine($"Typed...: {typedPassword}");
Console.WriteLine($"Hashed..: {testHash}");
Console.WriteLine($"The user is {(storedPasswordHash == testHash ? "" : "not ")}ok to log in.");

/// Generate hash to compare -- Correct case
typedPassword = clearPassword;
testHashGenerator = new Rfc2898DeriveBytes(typedPassword, storedSalt, storedIterationCount, hashAlgo);
testHashBytes = testHashGenerator.GetBytes(hashSizeInBytes);
testHash = Convert.ToBase64String(testHashBytes);

Console.WriteLine($"\n-- Correct attempt to log in --");
Console.WriteLine($"Typed...: {typedPassword}");
Console.WriteLine($"Hashed..: {testHash}");
Console.WriteLine($"The user is {(storedPasswordHash == testHash ? "" : "not ")}ok to log in.");
