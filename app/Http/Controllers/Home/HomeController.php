<?php

namespace App\Http\Controllers\Home;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;

class HomeController extends Controller
{
    public function index(){
        if (Auth::check()){
            return redirect()->route('dashboard');
        }
        else{
            return view('pages.home.home');
        }
        
    }
}
