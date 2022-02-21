# csharp-pwd-hashing
Experiments in password hashing

## Method

The hashing algorithm choice is PBKDF2, with SHA256 (310~320k iterations). It's ([OWASP and NIST recommended](https://cheatsheetseries.owasp.org/cheatsheets/Password_Storage_Cheat_Sheet.html#pbkdf2)).

The example uses [this implementation](https://docs.microsoft.com/pt-br/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-6.0).

### Salted-hash format (to persist)

`base64-salt|int-number-of-iterations|base64-hash`

## Notable methods

- `System.Text.Encoding.Unicode.GetBytes` converts string to byte array
- `System.Convert.ToHexString` converts string to hex string
- `System.Convert.ToBase64String` converts string to base64 string
- `System.Convert.FromBase64String` converts base64 string to string
- `System.Security.Cryptography.RandomNumberGenerator.GetInt32` generates random int
- `System.Security.Cryptography.RandomNumberGenerator.GetBytes` generates random byte array
- `System.Security.Cryptography.Rfc2898DeriveBytes` applies hash algorith with any number of iterations, and generates salt byte array and hash byte array

## Output example

```
-- Password ---
Clear...: My$upeR Secr37 _123✨
(Hex)...: 4D00790024007500700065005200200053006500630072003300370020005F003100320033002827
(Base64): TQB5ACQAdQBwAGUAUgAgAFMAZQBjAHIAMwA3ACAAXwAxADIAMwAoJw==

-- Rfc2898DeriveBytes ---
Random Salt (Hex).....: 858048E553BC58552B8F62944D37AE9A
Random Salt (Base64)..: hYBI5VO8WFUrj2KUTTeumg==

Hashing Algorithm.....: SHA256 (319676 iterations)
Hash (Hex)............: 5295307497A02B48AEEB6D944AF3FCFB332821A1FFC5F71A47313C4297268A91422D6A430609AB1081E4BE11E5CB25E9896C5324E73F6C5E27A4590D2E0F5D41
Hash (Base64).........: UpUwdJegK0iu622USvP8+zMoIaH/xfcaRzE8QpcmipFCLWpDBgmrEIHkvhHlyyXpiWxTJOc/bF4npFkNLg9dQQ==

-- Persistent password string --
hYBI5VO8WFUrj2KUTTeumg==|319676|UpUwdJegK0iu622USvP8+zMoIaH/xfcaRzE8QpcmipFCLWpDBgmrEIHkvhHlyyXpiWxTJOc/bF4npFkNLg9dQQ==

-- Incorrect attempt to log in --
Typed...: bad$robot
Hashed..: DFkuAS6jhnb8C3f2B1GP0n6LY7tBGckF62nEjH8cU8iWJLFa8LZX0fV2kUI9vGsttQvMJk+uoSTjQppr17lw1g==
The user is not ok to log in.

-- Correct attempt to log in --
Typed...: My$upeR Secr37 _123✨
Hashed..: UpUwdJegK0iu622USvP8+zMoIaH/xfcaRzE8QpcmipFCLWpDBgmrEIHkvhHlyyXpiWxTJOc/bF4npFkNLg9dQQ==
The user is ok to log in.
```
