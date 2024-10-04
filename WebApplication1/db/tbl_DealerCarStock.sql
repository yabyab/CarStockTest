CREATE TABLE DealerCarStock (
    stockid      INTEGER NOT NULL PRIMARY KEY,
    dealerid     INTEGER NOT NULL,
    model        TEXT NOT NULL,
    make         TEXT NOT NULL,
    year         INTEGER NOT NULL,
    price        DECIMAL(10,2) NOT NULL,
    quantity     INTEGER NOT NULL,
    create_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    update_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY(dealerid) REFERENCES Dealer(dealerid)
);
CREATE INDEX idx_dealercarstock_model ON DealerCarStock (model);
CREATE INDEX idx_dealercarstock_make ON DealerCarStock (make);
CREATE INDEX idx_dealercarstock_year ON DealerCarStock (year);
CREATE INDEX idx_dealercarstock_dealerid ON DealerCarStock (dealerid);
