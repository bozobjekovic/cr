# Company Registry Client

A frontend application for managing company data. This client provides a user-friendly interface to interact with the Company Registry backend API.

## Prerequisites

- Node.js 18+
- npm or yarn
- Docker (for containerized deployment)
- Company Registry API backend running

## Quick Start

### Development Setup

1. **Install dependencies:**

```bash
npm install
```

2. **Configure environment:**
   Copy the example environment file and update it:

```bash
cp .env.example .env
```

3. **Start development server:**

```bash
npm run dev
```

The application will be available at `http://localhost:5173`

### Docker Deployment

1. **Build the Docker image:**

```bash
docker build -t company-registry-client .
```

2. **Run the container:**

```bash
docker run -p 3000:80 company-registry-client
```

The application will be available at `http://localhost:3000`
