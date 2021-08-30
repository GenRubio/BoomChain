<li>
    <a href="#" data-toggle="dropdown" class="btn btn-primary dropdown-toggle get-started-btn mt-1 mb-1">Sign up</a>
    <ul class="dropdown-menu form-wrapper">
        <li>
            <form id="register">
                @csrf
                <p class="hint-text">Fill in this form to create your account!</p>
                <div class="form-group">
                    <input name="metamask" id="metamaskAccount" type="text" class="form-control"
                        placeholder="Metamask Account" required="required" value="{{ $metamask }}" readonly>
                </div>
                <div class="form-group">
                    <input name="password" type="password" class="form-control" placeholder="Password"
                        required="required">
                </div>
                <div class="form-group">
                    <input name="password_confirm" type="password" class="form-control" placeholder="Confirm Password"
                        required="required">
                </div>
                <div class="form-group">
                    <label class="checkbox-inline"><input type="checkbox" required="required"> I accept the <a
                            href="#">Terms &amp; Conditions</a></label>
                </div>
                <input type="submit" class="btn btn-primary btn-block" value="Sign up">
            </form>
        </li>
    </ul>
</li>

<script>
    $(document).ready(function() {
        $('#register').submit(function(event) {
            event.preventDefault();

            $.ajax({
                url: "{{ route('metamask.register') }}",
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
