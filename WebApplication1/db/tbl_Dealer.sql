CREATE TABLE Dealer(
  dealerid     INTEGER NOT NULL PRIMARY KEY,
  dealername   TEXT NOT NULL,
  dealeremail  TEXT NOT NULL,
  create_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
  update_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_dealer_dealername ON Dealer (dealername);
CREATE INDEX idx_dealer_dealeremail ON Dealer (dealeremail);