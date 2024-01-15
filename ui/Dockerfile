FROM node:18-alpine AS build
ENV NODE_ENV=production
WORKDIR /app
COPY . .
RUN npm ci --include=dev
RUN npm run build

FROM node:18-alpine AS runtime
ENV NODE_ENV=production
WORKDIR /app

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs

COPY --from=build /app ./
USER nextjs

CMD ["npm", "run", "start"]
