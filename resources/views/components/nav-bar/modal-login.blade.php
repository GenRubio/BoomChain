<li id="logginDiv">
    <a href="#" data-toggle="dropdown" class="btn btn-primary dropdown-toggle get-started-btn mt-1 mb-1">Login</a>
    <ul class="dropdown-menu form-wrapper">
        <li>
            <form id="login-web">
                @csrf
                <p class="hint-text">Sign in with your social media account</p>
                <div class="form-group social-btn clearfix" style="text-align: center">
                    <a href="#" class="btn btn-light" style="color:black">
                        <img src="https://avatars.githubusercontent.com/u/11744586?s=200&v=4"></i>
                        Metamask</a>

                </div>
                <div class="or-seperator"><b></b></div>
                <div class="form-group">
                    <input type="text" name="account" class="form-control" value="{{ $metamask }}" placeholder="Metamask"
                        required="required">
                </div>
                <div class="form-group">
                    <input name="password" type="password" class="form-control" placeholder="Password" required="required">
                </div>
                <input type="submit" class="btn btn-primary btn-block" value="Login">
                <div class="form-footer">
                    <a href="#">Forgot Your password?</a>
                </div>
            </form>
        </li>
    </ul>
</li>

<script>
    $(document).ready(function() {
        $('#login-web').submit(function(event) {
            event.preventDefault();

            $.ajax({
                url: "{{ route('metamask.login.web') }}",
                method: "POST",
                data: $(this).serialize(),
                success: function(data) {
                    if (data.success) {
                        location.href = "{{ route('dashboard') }}";
                    } else {
                        toastr.options.closeButton = true;
                        toastr.error(data.message);
                    }
                }
            })
        })
    })
</script>
