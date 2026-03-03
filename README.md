# ⚡ ProbEdge

> *"See the signal, take the edge."*

Real-time prediction market intelligence platform built with C# and Blazor.

![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat-square)
![Blazor](https://img.shields.io/badge/Blazor-Server-blue?style=flat-square)
![Polymarket](https://img.shields.io/badge/API-Polymarket-green?style=flat-square)
![License](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)

## 🚀 What is ProbEdge?

ProbEdge is a real-time prediction market analytics dashboard that helps traders identify opportunities, track probability movements, and detect arbitrage across multiple prediction markets.

## ✨ Features

- 📊 **Live Markets** — Real-time data from Polymarket API
- ⚡ **Signal Detection** — Automatic buy/drop signal generation based on probability movements
- 🔍 **Arbitrage Detector** — Price discrepancies between Polymarket and Kalshi
- 🕐 **Live Timeline** — Real-time probability movement feed
- 💬 **Community Insights** — Pro member analysis and comments
- 🔄 **Auto-refresh** — Data updates every 30 seconds

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core Blazor Server (.NET 10)
- **Language:** C#
- **API:** Polymarket Gamma API
- **Libraries:** Newtonsoft.Json, SignalR
- **Styling:** Custom CSS (dark theme)

## 📦 Installation
```bash
# Clone the repository
git clone https://github.com/ZekiGuzel/ProbEdge.git

# Navigate to project
cd ProbEdge

# Run the application
dotnet run
```

Open your browser at `http://localhost:5037`

## 📈 Data Sources

| Source | Type | Status |
|--------|------|--------|
| Polymarket | Live Markets | ✅ Active |
| Kalshi | Arbitrage Reference | 🔜 Coming Soon |
| Reddit | Social Sentiment | 🔜 Coming Soon |

## 🗺️ Roadmap

- [x] Live Polymarket data integration
- [x] Signal detection system
- [x] Arbitrage detector UI
- [x] Community comments
- [ ] Kalshi API integration
- [ ] Reddit sentiment analysis
- [ ] Pro membership system
- [ ] Mobile responsive design
- [ ] Price alerts via email

## ⚠️ Disclaimer

ProbEdge is an analytics tool only. Nothing on this platform constitutes financial advice. Always do your own research before making any trades.

## 👨‍💻 Author

**Zeki Güzel** — [@ZekiGuzel](https://github.com/ZekiGuzel)

---

*Built with ❤️ and C#*
```

Kaydet! Sonra terminale:
```
git add README.md
git commit -m "Add professional README"
git push
💪😎linkedin imi de ekle oraya05:18README.md'de şu kısmı bul:
markdown**Zeki Güzel** — [@ZekiGuzel](https://github.com/ZekiGuzel)
Şununla değiştir:
markdown**Zeki Güzel** — [@ZekiGuzel](https://github.com/ZekiGuzel) | [LinkedIn](https://www.linkedin.com/in/zeki-güzel-9a12003b3/)
```

Kaydet, sonra terminale:
```
git add README.md
git commit -m "Add LinkedIn to README"
git push
💪baaştan yazdır daha açıklayıcı ve uzun olsun ilk yazdığını beğendim + linkedin imi de ekle05:19VS Code'da README.md'deki her şeyi sil ve şunu yapıştır:
markdown# ⚡ ProbEdge
> *"See the signal, take the edge."*

**ProbEdge** is a real-time prediction market intelligence platform built entirely in C# and Blazor. It aggregates live data from prediction markets, detects probability movements, identifies arbitrage opportunities, and generates actionable trading signals — all in a sleek, dark-themed dashboard.

![.NET](https://img.shields.io/badge/.NET-10.0-purple?style=flat-square)
![Blazor](https://img.shields.io/badge/Blazor-Server-blue?style=flat-square)
![Polymarket](https://img.shields.io/badge/API-Polymarket-green?style=flat-square)
![C#](https://img.shields.io/badge/Language-C%23-239120?style=flat-square)
![License](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)

---

## 🧠 What is ProbEdge?

Prediction markets are one of the most accurate forecasting tools in the world. Platforms like **Polymarket** and **Kalshi** allow users to trade on real-world outcomes — elections, economics, crypto, geopolitics, and more.

The problem? The data is overwhelming. Prices move fast. Opportunities disappear in seconds.

**ProbEdge solves this.** It monitors thousands of markets in real time, automatically detects significant probability movements, finds price discrepancies between platforms, and surfaces the most actionable insights — so you can focus on making decisions, not hunting for data.

---

## ✨ Features

### 📊 Live Market Dashboard
- Real-time data pulled directly from the Polymarket Gamma API
- Markets sorted by 24-hour trading volume
- Color-coded probability bars (green = high, yellow = medium, red = low)
- Category filtering: Politics, Crypto, Economics, Tech, Sports, Geopolitics

### ⚡ Signal Detection Engine
- Automatically detects probability spikes and drops
- BUY SIGNAL — when probability rises significantly
- DROP SIGNAL — when probability falls sharply
- Signals ranked by movement strength
- Updates every 30 seconds

### 🔍 Arbitrage Detector
- Identifies price discrepancies between Polymarket and Kalshi
- Calculates the edge percentage for each opportunity
- Highlights the most profitable arbitrage windows
- Helps traders lock in risk-free profits

### 🕐 Live Timeline
- Chronological feed of all probability movements
- Color-coded dots: green (up), red (down), blue (neutral)
- Shows market category, time, and change percentage

### 💬 Community Insights
- Pro members can post market analysis
- Community-driven sentiment layer
- Real-time comment feed

### 🔄 Auto-Refresh System
- Market data refreshes every 30 seconds automatically
- No need to reload the page
- Live signal dot indicator in the navbar

---

## 🛠️ Tech Stack

| Technology | Purpose |
|-----------|---------|
| C# (.NET 10) | Core language |
| ASP.NET Core Blazor Server | Web framework |
| Polymarket Gamma API | Live market data |
| Newtonsoft.Json | JSON parsing |
| SignalR | Real-time updates |
| Custom CSS | Dark theme UI |

---

## 📦 Installation & Setup
```bash
# 1. Clone the repository
git clone https://github.com/ZekiGuzel/ProbEdge.git

# 2. Navigate to project directory
cd ProbEdge

# 3. Restore dependencies
dotnet restore

# 4. Run the application
dotnet run
```

Open your browser and go to:
```
http://localhost:5037
```

No API keys required — ProbEdge uses Polymarket's public Gamma API.

---

## 📈 Data Sources

| Source | Data Type | Status |
|--------|-----------|--------|
| Polymarket Gamma API | Live markets, prices, volume | ✅ Active |
| Kalshi | Arbitrage reference prices | 🔜 Coming Soon |
| Reddit API | Social sentiment analysis | 🔜 Coming Soon |
| Twitter/X | Social sentiment analysis | 🔜 Coming Soon |

---

## 🗺️ Roadmap

### ✅ Completed
- [x] Real-time Polymarket API integration
- [x] Live signal detection system (BUY / DROP)
- [x] Arbitrage detector UI
- [x] Live probability timeline
- [x] Community analysis comments
- [x] Auto-refresh every 30 seconds
- [x] Dark theme professional UI

### 🔜 Coming Soon
- [ ] Kalshi API live integration
- [ ] Reddit sentiment analysis
- [ ] Pro membership system ($9.99/month)
- [ ] Email price alerts
- [ ] Mobile responsive design
- [ ] Historical probability charts
- [ ] Portfolio tracker
- [ ] Telegram bot notifications

---

## 💡 How to Use ProbEdge

1. **Dashboard** — Get an overview of the hottest markets and live signals
2. **Markets** — Browse all active prediction markets sorted by volume
3. **Signals** — See the strongest buy and drop signals right now
4. **Arbitrage** — Find price gaps between Polymarket and Kalshi
5. **Timeline** — Follow real-time probability movements chronologically

---

## ⚠️ Disclaimer

ProbEdge is a **data analytics and visualization tool only**. Nothing on this platform constitutes financial or investment advice. Prediction market trading involves risk. Always conduct your own research before making any trades. The developers of ProbEdge are not responsible for any financial losses.

---

## 👨‍💻 Author

**Zeki Güzel**

[![GitHub](https://img.shields.io/badge/GitHub-ZekiGuzel-181717?style=flat-square&logo=github)](https://github.com/ZekiGuzel)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Zeki%20G%C3%BCzel-0077B5?style=flat-square&logo=linkedin)](https://www.linkedin.com/in/zeki-güzel-9a12003b3/)

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

*Built with ❤️ and C# — From the sea to the screen, always learning, always growing.*