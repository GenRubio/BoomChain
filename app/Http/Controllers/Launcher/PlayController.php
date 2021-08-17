<?php

namespace App\Http\Controllers\Launcher;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;


class PlayController extends Controller
{
    public function index()
    {
        return view('launcher.play');
    }

    public function logOut()
    {
        if (Auth::check()) {
            Auth::logout();
        }
        return redirect()->route('launcher.play');
    }
}
