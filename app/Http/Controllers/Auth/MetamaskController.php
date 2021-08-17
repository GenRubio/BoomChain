<?php

namespace App\Http\Controllers\Auth;

use App\Http\Controllers\Controller;
use App\Models\Usuario;
use Illuminate\Http\Request;
use Illuminate\Http\Response;
use Illuminate\Support\Facades\Auth;

class MetamaskController extends Controller
{
    public function login(Request $request){
        $user = Usuario::where('metamask', $request->account)->first();

        if ($user){
            Auth::login($user);
            $success = true;
        }
        else{
            $success = false;
        }
        return response()->json([
            'success' => $success,
        ], Response::HTTP_CREATED);
    }
}
