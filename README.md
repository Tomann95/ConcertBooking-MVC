# 🎵 ConcertBooking - System Rezerwacji Biletów

Nowoczesna aplikacja webowa oparta na wzorcu architektonicznym MVC (Model-View-Controller), służąca do przeglądania wydarzeń muzycznych oraz rezerwacji biletów.

---

## 📸 Zrzuty ekranu

![Strona Główna]
<img width="1892" height="880" alt="image" src="https://github.com/user-attachments/assets/6fe4da5a-01b9-4096-aa70-b9416918dfc7" />



![Wydarzenia i Wyszukiwarka]
<img width="1918" height="870" alt="image" src="https://github.com/user-attachments/assets/410c0c74-08c6-466a-8623-215619978e7c" />


![Panel Administratora]
<img width="1893" height="863" alt="image" src="https://github.com/user-attachments/assets/1fc35778-a1a7-4fd0-b820-ae38b1811fee" />


![Kalendarz]
<img width="1891" height="874" alt="image" src="https://github.com/user-attachments/assets/bba7b75b-46f3-4680-a969-758a4fd4d488" />

---

## ✨ Główne funkcjonalności

**Moduł Użytkownika:**
* Rejestracja i autoryzacja konta (ASP.NET Core Identity).
* Przeglądanie katalogu koncertów wraz ze szczegółowymi opisami artystów.
* Dynamiczna wyszukiwarka wydarzeń działająca bez przeładowywania strony.
* Interaktywny kalendarz koncertów ułatwiający planowanie.
* Rezerwacja biletów z inteligentnym, automatycznym pomniejszaniem puli dostępnych miejsc.
* Panel zarządzania "Moje Bilety" pozwalający na podgląd statusów i anulowanie rezerwacji.

**Moduł Administratora:**
* Dedykowany, zabezpieczony rolami Panel Admina.
* Dashboard ze statystykami biznesowymi na żywo (bilety oczekujące i zatwierdzone).
* Pełny system zarządzania ofertą (CRUD: dodawanie, edycja i usuwanie wydarzeń).
* Narzędzia do moderacji.

---

## 🚀 Zastosowane Technologie

* **Backend:** C#, ASP.NET Core MVC
* **Baza danych:** Microsoft SQL Server, Entity Framework Core
* **Autoryzacja i Zabezpieczenia:** ASP.NET Core Identity, walidacja formularzy
* **Frontend:** HTML5, CSS3, Bootstrap 5
* **JavaScript:** AJAX, FullCalendar.js, jQuery Validation
* **Architektura:** Wstrzykiwanie zależności, Serwisy 

---

## 🛠️ Jak uruchomić projekt lokalnie

1. Sklonuj repozytorium na swój komputer.
2. Otwórz plik rozwiązania (`ConcertBooking.sln`) w programie Visual Studio.
3. Upewnij się, że w pliku `appsettings.json` znajduje się poprawny tzw. Connection String do Twojej lokalnej bazy danych.
4. Otwórz Konsolę menedżera pakietów i wpisz komendę: `Update-Database`.
5. Uruchom aplikację klawiszem F5. System automatycznie utworzy bazę i wczyta niezbędne dane początkowe.
