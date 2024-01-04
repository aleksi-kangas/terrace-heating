FROM node:18-alpine AS build
WORKDIR /app
COPY . ./
RUN npm ci
RUN npm run build

FROM node:18-alpine AS runtime
WORKDIR /app

RUN addgroup --system --gid 1001 nodejs
RUN adduser --system --uid 1001 nextjs
USER nextjs

COPY --from=build /app/public ./
COPY --from=build --chown=nextjs:nodejs /app/.next/standalone ./
COPY --from=build /app/.next/static ./.next/static

CMD ["node", "server.js"]
