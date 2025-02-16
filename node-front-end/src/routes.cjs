
const { Router } = require('express');

const router = Router();

const HomeController = require('./controllers/home-controller.cjs');
const LoginController = require('./controllers/login-controller.cjs');
const SigninController = require('./controllers/signin-controller.cjs');

router.get('/', HomeController.index);
router.get('/login', LoginController.index);
router.get('/signin', SigninController.index);

module.exports = router;
