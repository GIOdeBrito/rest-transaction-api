
const express = require('express');
const expressLayouts = require('express-ejs-layouts');
const session = require('express-session');
const path = require('node:path');

const app = express();

const routes = require('./routes/routes.cjs');

// EJS view engine
app.set('view engine', 'ejs');

// Views path
app.set('views', path.join(__dirname, 'views'));

// Express layout
app.use(expressLayouts);
app.set('layout', path.join(__dirname, 'layout/main.ejs'));

// Use static public folder
app.use('/public', express.static(path.join(__dirname, '../public')));

// For parsing form data
app.use(express.urlencoded({ extended: true }));

// For parsing JSON
app.use(express.json());

// Use session for the application
app.use(session({
	secret: process.env?.SESSION_SECRET,
	resave: false,
	saveUninitialized: false,
	cookie: {
		maxAge: 3000,
		httpOnly: true,
		secure: false
	}
}));

// Custom routes
app.use(routes);

const PORT = 8080;
app.listen(PORT, () => console.log(`App running on Port::${PORT}`));
