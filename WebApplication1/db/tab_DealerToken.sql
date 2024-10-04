CREATE TABLE DealerToken(
  id           INTEGER NOT NULL PRIMARY KEY,
  dealerid     INTEGER NOT NULL,
  token        TEXT NOT NULL,
  expired_at   TIMESTAMP DEFAULT NULL,
  create_at    TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_dealertoken_token ON DealerToken (token);
CREATE INDEX idx_dealertoken_dealerid ON DealerToken (dealerid);