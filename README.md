<div align="center">

<br/>

```
 ██╗ ██████╗██╗   ██╗███████╗████████╗ ██████╗ ███╗   ███╗███████╗██████╗ 
 ██║██╔════╝██║   ██║██╔════╝╚══██╔══╝██╔═══██╗████╗ ████║██╔════╝██╔══██╗
 ██║██║     ██║   ██║███████╗   ██║   ██║   ██║██╔████╔██║█████╗  ██████╔╝
 ██║██║     ██║   ██║╚════██║   ██║   ██║   ██║██║╚██╔╝██║██╔══╝  ██╔══██╗
 ██║╚██████╗╚██████╔╝███████║   ██║   ╚██████╔╝██║ ╚═╝ ██║███████╗██║  ██║
 ╚═╝ ╚═════╝ ╚═════╝ ╚══════╝   ╚═╝    ╚═════╝ ╚═╝     ╚═╝╚══════╝╚═╝  ╚═╝
```

### Customer Management Desktop App · WPF + C# + MVVM

<br/>

![WPF](https://img.shields.io/badge/WPF-.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![MVVM](https://img.shields.io/badge/Pattern-MVVM-blueviolet?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-Completed-brightgreen?style=for-the-badge)
![Type](https://img.shields.io/badge/Type-Academic%20Project-orange?style=for-the-badge)

<br/>

> 🎓 **Academic project** — M0488 · Interface Development · TEKNÓS Campus Professional UVIC·UCC · 2025–2026

<br/>

</div>

---

## 📌 What is iCustomer?

**iCustomer** is a desktop application built with **WPF and C#** that allows users to manage a customer list in a clean, modern dark-themed interface. It was developed following the **MVVM architectural pattern**, strictly separating UI from business logic through Data Binding.

All data is fictional and generated within the app itself.

---

## ✨ Features

| Feature | Description |
|--------|-------------|
| 📋 **Customer List** | Full DataGrid with per-row action buttons (view, edit, delete) |
| ➕ **Add / Edit** | Form with real-time field validation and a Save button that only enables when all fields are valid |
| 🗑️ **Delete** | Confirmation modal to prevent accidental deletions |
| 📊 **Monthly Chart** | Bar chart showing a customer's monthly spending across 12 months (with seasonal weighting) |
| 🔄 **Chart Comparison** ⭐ | Select a second customer and overlay both charts in the same view |
| 🌙 **Dark Theme** | Global dark visual theme applied via `DarkTheme.xaml` |
| 🧭 **SPA Navigation** | Internal view switching via `ContentControl` — no window reloads |

---

## 🏗️ Architecture — MVVM

```
iCustomer/
│
├── Models/              → Customer.cs (data + fictitious expense generation)
├── ViewModels/          → One ViewModel per view + MainViewModel (central coordinator)
├── Views/               → .xaml screens (CustomerView, ChartView, FormView...)
│   └── Themes/          → DarkTheme.xaml (global visual theme)
├── MainWindow.xaml      → SPA container using ContentControl
└── App.xaml             → Global configuration & theme loading
```

> The **View (XAML) contains zero logic** — everything flows through ViewModels via Binding. The View observes data and redraws itself automatically on change.

## ⚙️ Form Validation Rules

| Field | Rule |
|-------|------|
| Name / Surname | Required · Min. 3 characters |
| DNI | Format: 8 digits + 1 letter (e.g. `12345678Z`) |
| Email | Valid format via Regular Expression |
| Phone | Digits only · Min. 9 characters |
| Registration Date | Built-in DatePicker |

---

## 📦 Tech Stack

- **Language:** C# (.NET)
- **UI Framework:** WPF (Windows Presentation Foundation)
- **Pattern:** MVVM (Model – View – ViewModel)
- **Charts:** LiveCharts (NuGet)
- **Collections:** `ObservableCollection<T>` for reactive DataGrid updates
- **Commands:** `RelayCommand` pattern

---

## 🎓 Academic Context

This project was developed as **PAC #1** for the subject **M0488 — Interface Development** at TEKNÓS Campus Professional (UVIC·UCC), during the 2025–2026 academic year, under the supervision of professor **David González**.

---

<div align="center">

Made with ☕ and a lot of `INotifyPropertyChanged`

</div>
