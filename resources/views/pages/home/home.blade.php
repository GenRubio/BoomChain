@extends('layouts.app')

@section('personal-style')

@endsection

@section('content')
    
    <h1>Hola</h1>
@endsection

@section('personal-script')
    <script>
        $(document).ready(function() {
            window.ethereum.on('accountsChanged', function(accounts) {
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
                    url: "{{ route('metamask.index') }}",
                    method: "POST",
                    data: {
                        "_token": "{{ csrf_token() }}",
                        "account": account
                    },
                    success: function(data) {
                        $('#metamask-status').html(data.content);
                    }
                })
            }
        });
    </script>
@endsection
