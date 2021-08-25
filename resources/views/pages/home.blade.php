@extends('layouts.app')

@section('personal-style')

@endsection

@section('content')
    @include('components.nav-bar')
    <h1>Hola</h1>
@endsection

@section('personal-script')
    <script>
        //Show or hide loggin DIV
        var showHideLog = false;
        var showHideRegister = false;


      


        $(document).ready(function() {

            //Detect if metamask account changed
              window.ethereum.on('accountsChanged', function (accounts) {
                   getAccount();
          })

            if (typeof window.ethereum !== 'undefined') {
                console.log('MetaMask is installed!');
                getAccount();
               

            } else {
                alert("Metamask is not installed")
            }
            

            async function getAccount() {
                const accounts = await ethereum.request({
                    method: 'eth_requestAccounts'
                });
                const account = accounts[0];
                
                $.ajax({
                    url: "{{ route('metamask.login') }}",
                    method: "POST",
                    data: {
                        "_token": "{{ csrf_token() }}",
                        "account": account
                    },
                    success:function(data){
                        console.log("cuenta metamaks->", account);
                        //User don't have account
                        if (data.success){
                            alert("OK");
                            showHideLog = true;
                            showHideRegister = false;
                           
                        }
                        else{
                            alert("Don't have registered account for this metamask");
                            showHideLog = false;
                            showHideRegister = true
                            //Add metamask account and make it only readable
                            document.getElementById("metamaskAccount").value = account;
                            document.getElementById("metamaskAccount").readOnly = true;
                        }
                    }
                })

            }
        });
        console.log(showHideLog)

        //Show / Hide Loggin Function
        function showLoggin() {
            var x = document.getElementById("logginDiv");
            if (showHideLog == true) {
                x.style.display = "block";
            } else {
                x.style.display = "none";
            }
        }

        showLoggin();


    </script>
@endsection
