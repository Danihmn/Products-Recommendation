# Products Recommendations

A small, practical demo of **RAG (Retrieval-Augmented Generation)** applied to product recommendations — no LLM generation involved, just the retrieval half.

## Intent

The goal of this project is to show, in the simplest way possible, how vector search works in practice:

1. Products are stored in Postgres.
2. Each product gets an embedding (via a local Ollama model) generated from its category, and stored as a vector using `pgvector`.
3. A user prompt is embedded the same way, then compared against the stored vectors using cosine distance to find the closest matching products.

This is essentially the "R" in RAG on its own: turning text into embeddings and doing a similarity search over them, without an LLM generating a final answer.

## Stack

- ASP.NET Core (minimal APIs)
- Entity Framework Core + Npgsql
- PostgreSQL with the `pgvector` extension
- Ollama (`mxbai-embed-large`) for generating embeddings, via Semantic Kernel

## Endpoints

- `POST /v1/products` — creates a product and its corresponding embedding/recommendation entry.
- `GET /v1/seed` — generates embeddings for existing products that don't have one yet.
- `POST /v1/prompt` — embeds a free-text prompt and returns the top matching products by vector similarity.
