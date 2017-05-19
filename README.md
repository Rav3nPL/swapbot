# swapbot

Bot do obsługi swapów na giełdzie Bitmarket

Na koncie Bitmarket: konto/przegląd/dostęp API

link: https://www.bitmarket.pl/apikeys.php

należy wygenerować nowy klucz z dostępem:

- Pobranie wartości sald na koncie
- Pobranie listy kontraktów swap
- Otwarcie kontraktu swap
- Zamknięcie kontraktu swap

i zapisać sobie klucz publiczny i prywatny

Po odpaleniu apki wklejamy klucze i wciskamy "sprawdź" - pojawi się komunikat że ok.

Wybieramy sobie jak blisko granicy opłacalności chcemy być (im mamy więcej tym dalej) i co jaki czas sprawdzamy.

Po odpaleniu program będzie okresowo sprawdzał akatualny stan i w razie potrzeby korygował naszą pozycję żeby (prawie) zawsze zarabiała.

Bot "zakłada" że masz jakieś środki na koncie oraz masz max 1 kontrakt swap.

Do działania potrzebny jest plik "swapbot.exe" oraz bibioteka "Newtonsoft.Json.dll" - oba zawarte w katalogu "bin/Release". Bilbioteka musi być w tym samym katalogu co plik exe. Zapis konfiguracji tworzy plik "konfig.txt". Są to jedyne pliki używane przez program.

Dyskusja na forum: https://forum.bitcoin.pl/viewtopic.php?f=54&t=20230

Działa? Zarobiłeś więcej niż wcześniej? Tipbox: 1Rav3nkMayCijuhzcYemMiPYsvcaiwHni ;]
