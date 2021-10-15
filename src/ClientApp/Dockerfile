FROM node:lts as builder

WORKDIR /app

COPY . .

RUN npm install

RUN npm run build

RUN rm -rf node_modules && \
  NODE_ENV=production npm ci \
  --only=production
FROM node:lts

WORKDIR /app

COPY --from=builder /app  .

ENV HOST 0.0.0.0
ENV PORT 3000
EXPOSE 3000

CMD [ "npm", "run", "start" ]
