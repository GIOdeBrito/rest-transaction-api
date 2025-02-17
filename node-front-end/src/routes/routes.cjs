
const { Router } = require('express');

const router = Router();

const HomeController = require('../controllers/home-controller.cjs');
const LoginController = require('../controllers/login-controller.cjs');
const SigninController = require('../controllers/signin-controller.cjs');
const TestController = require('../controllers/test-controller.cjs');

router.get('/', HomeController.index);
router.get('/login', LoginController.index);
router.get('/signin', SigninController.index);

router.post('/api/v1/login', LoginController.login);

if(process.env?.IS_DEVELOPMENT === 'true')
{
	router.get('/api/v1/test/asp', TestController.request);
}

module.exports = router;
