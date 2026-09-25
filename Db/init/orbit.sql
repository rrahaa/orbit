-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Erstellungszeit: 03. Sep 2026 um 11:48
-- Server-Version: 10.4.27-MariaDB
-- PHP-Version: 7.4.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Datenbank: `orbit`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `artikel`
--

CREATE TABLE `artikel` (
                           `artikel_id` int(10) UNSIGNED NOT NULL,
                           `kategorie_id` int(10) UNSIGNED NOT NULL,
                           `name` varchar(100) NOT NULL,
                           `preis` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `artikel`
--

INSERT INTO `artikel` (`artikel_id`, `kategorie_id`, `name`, `preis`) VALUES
                                                                          (1, 1, 'Cola', '3.50'),
                                                                          (2, 1, 'Wasser', '2.50'),
                                                                          (3, 1, 'Apfelschorle', '3.20'),
                                                                          (4, 2, 'Bier (Pils)', '4.50'),
                                                                          (5, 2, 'Weizenbier', '4.80'),
                                                                          (6, 3, 'Hauswein Rot', '5.50'),
                                                                          (7, 3, 'Hauswein Weiß', '5.50'),
                                                                          (8, 4, 'Gin Tonic', '7.50'),
                                                                          (9, 5, 'Kaffee', '3.00'),
                                                                          (10, 5, 'Tee', '2.80');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `artikel_kategorie`
--

CREATE TABLE `artikel_kategorie` (
                                     `kategorie_id` int(10) UNSIGNED NOT NULL,
                                     `kategoriename` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `artikel_kategorie`
--

INSERT INTO `artikel_kategorie` (`kategorie_id`, `kategoriename`) VALUES
                                                                      (2, 'Bier'),
                                                                      (5, 'Heißgetränke'),
                                                                      (1, 'Softdrinks'),
                                                                      (4, 'Spirituosen'),
                                                                      (3, 'Wein & Sekt');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `bestellposition`
--

CREATE TABLE `bestellposition` (
                                   `bestellposition_id` int(10) UNSIGNED NOT NULL,
                                   `bestellung_id` int(10) UNSIGNED NOT NULL,
                                   `artikel_id` int(10) UNSIGNED NOT NULL,
                                   `menge` int(10) UNSIGNED NOT NULL,
                                   `einzelpreis` decimal(10,2) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `bestellposition`
--

INSERT INTO `bestellposition` (`bestellposition_id`, `bestellung_id`, `artikel_id`, `menge`, `einzelpreis`) VALUES
                                                                                                                (1, 1, 1, 2, '3.50'),
                                                                                                                (2, 1, 4, 1, '4.50'),
                                                                                                                (3, 1, 8, 2, '7.50'),
                                                                                                                (4, 2, 2, 2, '2.50'),
                                                                                                                (5, 2, 9, 1, '3.00'),
                                                                                                                (6, 3, 3, 2, '3.20'),
                                                                                                                (7, 3, 5, 1, '4.80'),
                                                                                                                (8, 4, 6, 1, '5.50');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `bestellung`
--

CREATE TABLE `bestellung` (
                              `bestellung_id` int(10) UNSIGNED NOT NULL,
                              `tisch_id` int(10) UNSIGNED NOT NULL,
                              `mitarbeiter_id` int(10) UNSIGNED NOT NULL,
                              `bestell_status_id` int(10) UNSIGNED NOT NULL,
                              `bestelldatum` datetime NOT NULL DEFAULT current_timestamp(),
                              `abschlusszeitpunkt` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `bestellung`
--

INSERT INTO `bestellung` (`bestellung_id`, `tisch_id`, `mitarbeiter_id`, `bestell_status_id`, `bestelldatum`, `abschlusszeitpunkt`) VALUES
                                                                                                                                        (1, 3, 1, 4, '2026-09-01 12:15:00', NULL),
                                                                                                                                        (2, 1, 1, 5, '2026-09-01 13:10:00', '2026-09-01 13:55:00'),
                                                                                                                                        (3, 6, 1, 3, '2026-09-02 18:20:00', NULL),
                                                                                                                                        (4, 2, 1, 1, '2026-09-03 09:30:00', NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `bestell_status`
--

CREATE TABLE `bestell_status` (
                                  `bestell_status_id` int(10) UNSIGNED NOT NULL,
                                  `statusname` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `bestell_status`
--

INSERT INTO `bestell_status` (`bestell_status_id`, `statusname`) VALUES
                                                                     (5, 'Bezahlt'),
                                                                     (3, 'Fertig'),
                                                                     (2, 'In Bearbeitung'),
                                                                     (1, 'Offen'),
                                                                     (4, 'Serviert');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `mitarbeiter`
--

CREATE TABLE `mitarbeiter` (
                               `mitarbeiter_id` int(10) UNSIGNED NOT NULL,
                               `rolle_id` int(10) UNSIGNED NOT NULL,
                               `name` varchar(100) NOT NULL,
                               `benutzername` varchar(50) NOT NULL,
                               `passwort_hash` varchar(255) NOT NULL,
                               `ist_aktiv` tinyint(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `mitarbeiter`
--

INSERT INTO `mitarbeiter` (`mitarbeiter_id`, `rolle_id`, `name`, `benutzername`, `passwort_hash`, `ist_aktiv`) VALUES
                                                                                                                   (1, 1, 'Anna Müller', 'anna.service', 'TEST_HASH_001', 1),
                                                                                                                   (2, 2, 'Max Schmidt', 'max.bar', 'TEST_HASH_002', 1),
                                                                                                                   (4, 4, 'Tom Fischer', 'tom.admin', 'TEST_HASH_004', 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `rolle`
--

CREATE TABLE `rolle` (
                         `rolle_id` int(10) UNSIGNED NOT NULL,
                         `rollenname` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `rolle`
--

INSERT INTO `rolle` (`rolle_id`, `rollenname`) VALUES
                                                   (4, 'Administration'),
                                                   (2, 'Bar'),
                                                   (1, 'Service');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tisch`
--

CREATE TABLE `tisch` (
                         `tisch_id` int(10) UNSIGNED NOT NULL,
                         `tisch_status_id` int(10) UNSIGNED NOT NULL,
                         `tischnummer` int(10) UNSIGNED NOT NULL,
                         `kapazitaet` int(10) UNSIGNED NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `tisch`
--

INSERT INTO `tisch` (`tisch_id`, `tisch_status_id`, `tischnummer`, `kapazitaet`) VALUES
                                                                                     (1, 1, 1, 2),
                                                                                     (2, 1, 2, 4),
                                                                                     (3, 2, 3, 4),
                                                                                     (4, 1, 4, 6),
                                                                                     (5, 1, 5, 2),
                                                                                     (6, 2, 6, 8);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tisch_status`
--

CREATE TABLE `tisch_status` (
                                `tisch_status_id` int(10) UNSIGNED NOT NULL,
                                `statusname` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Daten für Tabelle `tisch_status`
--

INSERT INTO `tisch_status` (`tisch_status_id`, `statusname`) VALUES
                                                                 (2, 'besetzt'),
                                                                 (1, 'frei');

--
-- Indizes der exportierten Tabellen
--

--
-- Indizes für die Tabelle `artikel`
--
ALTER TABLE `artikel`
    ADD PRIMARY KEY (`artikel_id`),
  ADD KEY `kategorie_id` (`kategorie_id`);

--
-- Indizes für die Tabelle `artikel_kategorie`
--
ALTER TABLE `artikel_kategorie`
    ADD PRIMARY KEY (`kategorie_id`),
  ADD UNIQUE KEY `kategoriename` (`kategoriename`);

--
-- Indizes für die Tabelle `bestellposition`
--
ALTER TABLE `bestellposition`
    ADD PRIMARY KEY (`bestellposition_id`),
  ADD UNIQUE KEY `bestellung_id` (`bestellung_id`,`artikel_id`),
  ADD KEY `artikel_id` (`artikel_id`);

--
-- Indizes für die Tabelle `bestellung`
--
ALTER TABLE `bestellung`
    ADD PRIMARY KEY (`bestellung_id`),
  ADD KEY `tisch_id` (`tisch_id`),
  ADD KEY `mitarbeiter_id` (`mitarbeiter_id`),
  ADD KEY `bestell_status_id` (`bestell_status_id`);

--
-- Indizes für die Tabelle `bestell_status`
--
ALTER TABLE `bestell_status`
    ADD PRIMARY KEY (`bestell_status_id`),
  ADD UNIQUE KEY `statusname` (`statusname`);

--
-- Indizes für die Tabelle `mitarbeiter`
--
ALTER TABLE `mitarbeiter`
    ADD PRIMARY KEY (`mitarbeiter_id`),
  ADD UNIQUE KEY `benutzername` (`benutzername`),
  ADD KEY `rolle_id` (`rolle_id`);

--
-- Indizes für die Tabelle `rolle`
--
ALTER TABLE `rolle`
    ADD PRIMARY KEY (`rolle_id`),
  ADD UNIQUE KEY `rollenname` (`rollenname`);

--
-- Indizes für die Tabelle `tisch`
--
ALTER TABLE `tisch`
    ADD PRIMARY KEY (`tisch_id`),
  ADD UNIQUE KEY `tischnummer` (`tischnummer`),
  ADD KEY `tisch_status_id` (`tisch_status_id`);

--
-- Indizes für die Tabelle `tisch_status`
--
ALTER TABLE `tisch_status`
    ADD PRIMARY KEY (`tisch_status_id`),
  ADD UNIQUE KEY `statusname` (`statusname`);

--
-- AUTO_INCREMENT für exportierte Tabellen
--

--
-- AUTO_INCREMENT für Tabelle `artikel`
--
ALTER TABLE `artikel`
    MODIFY `artikel_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT für Tabelle `artikel_kategorie`
--
ALTER TABLE `artikel_kategorie`
    MODIFY `kategorie_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT für Tabelle `bestellposition`
--
ALTER TABLE `bestellposition`
    MODIFY `bestellposition_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT für Tabelle `bestellung`
--
ALTER TABLE `bestellung`
    MODIFY `bestellung_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `bestell_status`
--
ALTER TABLE `bestell_status`
    MODIFY `bestell_status_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT für Tabelle `mitarbeiter`
--
ALTER TABLE `mitarbeiter`
    MODIFY `mitarbeiter_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `rolle`
--
ALTER TABLE `rolle`
    MODIFY `rolle_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT für Tabelle `tisch`
--
ALTER TABLE `tisch`
    MODIFY `tisch_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT für Tabelle `tisch_status`
--
ALTER TABLE `tisch_status`
    MODIFY `tisch_status_id` int(10) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `artikel`
--
ALTER TABLE `artikel`
    ADD CONSTRAINT `artikel_ibfk_1` FOREIGN KEY (`kategorie_id`) REFERENCES `artikel_kategorie` (`kategorie_id`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `bestellposition`
--
ALTER TABLE `bestellposition`
    ADD CONSTRAINT `bestellposition_ibfk_1` FOREIGN KEY (`bestellung_id`) REFERENCES `bestellung` (`bestellung_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `bestellposition_ibfk_2` FOREIGN KEY (`artikel_id`) REFERENCES `artikel` (`artikel_id`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `bestellung`
--
ALTER TABLE `bestellung`
    ADD CONSTRAINT `bestellung_ibfk_1` FOREIGN KEY (`tisch_id`) REFERENCES `tisch` (`tisch_id`) ON UPDATE CASCADE,
  ADD CONSTRAINT `bestellung_ibfk_2` FOREIGN KEY (`mitarbeiter_id`) REFERENCES `mitarbeiter` (`mitarbeiter_id`) ON UPDATE CASCADE,
                                                                                                                                                                                                                                        ADD CONSTRAINT `bestellung_ibfk_3` FOREIGN KEY (`bestell_status_id`) REFERENCES `bestell_status` (`bestell_status_id`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `mitarbeiter`
--
ALTER TABLE `mitarbeiter`
    ADD CONSTRAINT `mitarbeiter_ibfk_1` FOREIGN KEY (`rolle_id`) REFERENCES `rolle` (`rolle_id`) ON UPDATE CASCADE;

--
-- Constraints der Tabelle `tisch`
--
ALTER TABLE `tisch`
    ADD CONSTRAINT `tisch_ibfk_1` FOREIGN KEY (`tisch_status_id`) REFERENCES `tisch_status` (`tisch_status_id`) ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
