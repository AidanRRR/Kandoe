FROM node:boron

COPY . /app

WORKDIR /app

RUN ["npm", "install", "express"]

RUN ["npm", "install", "serve-static"]

EXPOSE 3000

CMD ["node","Server.js"]
