# Laborator Curs 2 — UML (Sistem Gestiune Comenzi)

## Livrabile

| Fișier | Descriere |
|--------|-----------|
| [docs/use-case.png](docs/use-case.png) | Diagrama de cazuri de utilizare |
| [docs/class-diagram-before.png](docs/class-diagram-before.png) | Diagrama de clasă — structura actuală |
| [docs/class-diagram-after.png](docs/class-diagram-after.png) | Diagrama de clasă — structura propusă |
| [docs/sequence-place-order.md](docs/sequence-place-order.md) | Diagramă de secvență — plasare comandă (Mermaid) |
| [docs/sequence-cancel-order.md](docs/sequence-cancel-order.md) | Diagramă de secvență — anulare comandă (Mermaid) |

## Decizii de modelare

### Diagrama de cazuri de utilizare

- **Actori:** Client, Admin, Sistem de plată (extern).
- **Granița sistemului:** dreptunghiul *Sistem Gestiune Comenzi* grupează toate cazurile de utilizare.
- **Cazuri de utilizare (8):** plasare comandă, anulare comandă, consultare istoric, căutare produse, gestionare catalog, raport vânzări, actualizare status, procesare plată, procesare rambursare.
- **Include:** Plasare comandă include Procesare plată (plata este obligatorie la comandă nouă).
- **Extend:** Anulare comandă extinde Procesare rambursare (rambursarea este opțională, doar dacă plata a fost deja efectuată).

### Diagrama de clasă — înainte

- **OrderManager** este modelată ca **God Class**: validare, stoc, SQL, plată, email și rapoarte într-o singură clasă.
- Cuplarea directă la SqlClient și API-uri externe este marcată cu săgeți de dependență.
- **Order** compune **OrderLine**; **Order** asociază **Customer**.

### Diagrama de clasă — după

- **OrderController** delegă doar către **OrderService**.
- **OrderService** depinde de interfețe: IOrderRepository, IStockService, IPaymentGateway, IEmailService.
- Implementările concrete pot fi înlocuite fără modificarea logicii de business (SRP, DIP).

### Diagrame de secvență

- Ambele fluxuri modelează designul **actual** (God Class OrderManager).
- **Plasare comandă:** POST HTTP, validare, verificare stoc, plată, salvare în DB, email opțional de confirmare. Folosește blocuri `alt` și `opt`.
- **Anulare comandă:** DELETE HTTP, ramură `alt` când comanda este Shipped (409 Conflict) față de statusuri anulabile; `opt` pentru rambursare dacă plata există.
