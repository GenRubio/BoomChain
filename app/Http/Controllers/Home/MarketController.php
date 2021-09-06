<?php

namespace App\Http\Controllers\Home;

use App\Http\Controllers\Controller;
use App\Models\CatalagoObjeto;
use Illuminate\Http\Request;

class MarketController extends Controller
{
    public function index(){
        $items = CatalagoObjeto::where('visible', 1)
        ->where('active', 1)
        ->get();

        return view('pages.home.market.market', [
            'objetos' => $items,
        ]);
    }
}
