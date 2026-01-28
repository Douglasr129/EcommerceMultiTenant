CREATE TABLE product (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    price_amount NUMERIC(18,2) NOT NULL CHECK (price_amount > 0),
    price_currency VARCHAR(10) NOT NULL DEFAULT 'BRL',
    stock INTEGER NOT NULL DEFAULT 0 CHECK (stock >= 0),
    category_id UUID REFERENCES category(id) ON DELETE SET NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_product_name ON product(name);
CREATE INDEX idx_product_category_id ON product(category_id);