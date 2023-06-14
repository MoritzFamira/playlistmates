# Base image
FROM node:18-alpine

# Set working directory
WORKDIR /app

# Copy package.json and package-lock.json
COPY ./package*.json ./

# Install dependencies
RUN npm install

# Copy all other source files
COPY . .

# Build app
RUN npm run build

# Install serve
RUN npm install -g serve

# Run the app
CMD serve -l 80 -s build

# Expose port
EXPOSE 80