FROM node:lts-alpine3.14 as builder

WORKDIR /app

COPY . .

#RUN npm install -g yarn

RUN yarn install

RUN yarn build

RUN rm -rf node_modules && \
  yarn install --production=true
FROM node:lts

WORKDIR /app

COPY --from=builder /app  .

ENV HOST 0.0.0.0
ENV PORT 3000
EXPOSE 3000

CMD [ "npm", "run", "start" ]
