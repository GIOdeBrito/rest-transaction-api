FROM node:20.10.0

WORKDIR /usr/src/app

COPY node-front-end/package.json .
RUN npm install

COPY node-front-end/. .

CMD ["npm", "run", "start"]