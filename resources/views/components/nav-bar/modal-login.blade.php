<li id="logginDiv">
    <a href="#" data-toggle="dropdown" class="btn btn-primary dropdown-toggle get-started-btn mt-1 mb-1">Login</a>
    <ul class="dropdown-menu form-wrapper">					
        <li>
            <form action="/examples/actions/confirmation.php" method="post">
                <p class="hint-text">Sign in with your social media account</p>
                <div class="form-group social-btn clearfix" style="text-align: center">
                    <a href="#" class="btn btn-light" style="color:black">
                    <img src="https://avatars.githubusercontent.com/u/11744586?s=200&v=4"></i> 
                    Metamask</a>
                    
                </div>
                <div class="or-seperator"><b>or</b></div>
                <div class="form-group">
                    <input type="text" class="form-control" placeholder="Username" required="required">
                </div>
                <div class="form-group">
                    <input type="password" class="form-control" placeholder="Password" required="required">
                </div>
                <input type="submit" class="btn btn-primary btn-block" value="Login">
                <div class="form-footer">
                    <a href="#">Forgot Your password?</a>
                </div>
            </form>
        </li>
    </ul>
</li>