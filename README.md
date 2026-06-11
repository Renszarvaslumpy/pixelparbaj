# PixelPárbaj

**PixelPárbaj** (Pixel Duel) is an online movie poster guessing game for film enthusiasts. Movie posters are shown in a heavily pixelated form and gradually become clearer over time — your goal is to identify the film title as quickly as possible. The faster you guess, the better your score!

The game was created by the [FreakShow](https://www.youtube.com/@FreakShowHU) YouTube channel, a horror-themed channel from Hungary, as a passion project for movie lovers. The source code is now published publicly so that fans and developers can improve the game and make it even more fun.

---

## Game Modes

| Mode | Description |
|------|-------------|
| **Szóló Játék** (Solo) | Play alone against the clock |
| **Párbaj Játék** (Duel) | Challenge another player online |
| **Party Játék** (Party) | Compete in groups |
| **Pixeletelő** | A shared screen experience where one screen shows the posters and everyone guesses on their own phone |

### Difficulty Levels

- **Normál** – Standard challenge
- **Nehéz** – Hard
- **Mazochista** – Masochist
- **Lehetetlen** – Impossible

---

## Tech Stack

- **Framework:** ASP.NET Core (Razor Pages) — .NET 10
- **Database:** Microsoft SQL Server via Entity Framework Core
- **Other libraries:** Newtonsoft.Json, QRCoder, System.Drawing.Common

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A running **SQL Server** instance

### Configuration

1. Clone the repository:
   ```bash
   git clone https://github.com/Renszarvaslumpy/pixelparbaj.git
   cd pixelparbaj
   ```

2. Set the database connection string in `PixelParbaj_CORE/appsettings.json` (or use `appsettings.Development.json` for local development):
   ```json
   {
     "ConnectionStrings": {
       "PPConnectionString": "Server=YOUR_SERVER;Database=PixelParbaj;..."
     }
   }
   ```

3. Apply database migrations (if applicable):
   ```bash
   cd PixelParbaj_CORE
   dotnet ef database update
   ```

### Running the Application

```bash
dotnet run --project PixelParbaj_CORE
```

The application will be available at `https://localhost:5001` (or the port shown in the console).

### Building

```bash
dotnet build PixelParbaj_CORE.sln
```

### Running Tests

```bash
dotnet test PixelParbaj_CORE.sln
```

---

## Contributing

Contributions are very welcome! Whether you've found a bug, have a new feature idea, or just want to improve the code, feel free to:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/my-improvement`)
3. Commit your changes
4. Open a Pull Request

For questions or feedback you can also reach us at **freakshowhu@gmail.com**.

---

## License

See the [LICENSE](LICENSE) file for details.
