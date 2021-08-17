<?php

namespace App\Http\Controllers\Launcher;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;

class PlayController extends Controller
{
    public function index(){
        return view('launcher.play');
    }
}
