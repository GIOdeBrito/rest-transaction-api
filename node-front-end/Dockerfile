FROM node:20.10.0

WORKDIR /usr/src/app

COPY node-front-end/package.json .
RUN npm install

COPY node-front-end/. .
COPY node-front-end/.env .

CMD ["npm", "run", "start"]