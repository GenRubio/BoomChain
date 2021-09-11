@extends('layouts.app')

@section('content')
@include('partials.nav-bar')
    {{--TOP MENÚ  --}}
    <div style="top:70px; width:100%; position:fixed; border-bottom-width:1px; background-color: gray">
        <div style="display:inline-flex">
            <a style="opacity:1; position:relative; padding-left:20px; padding right:20x; padding-top: 16px; padding-bottom:20px; font-weight: 700">
                
                <img src="https://marketplace.axieinfinity.com/static/image/tab-axie.png" alt="">
                Personajes
                <div style="width:100%; left:0; bottom: 0; position:absolute; height: 4px; background-color: #046cfc;" class="absolute left-0 bottom-0 w-full h-4 bg-primary-3"></div>
            </a>

            <a style="opacity:1; position:relative; padding-left:20px; padding right:20x; padding-top: 16px; padding-bottom:20px; font-weight: 700">
                
                <img src="https://marketplace.axieinfinity.com/static/image/tab-land.png" alt="">
                Islas
            </a>

            <a style="opacity:1; position:relative; padding-left:20px; padding right:20x; padding-top: 16px; padding-bottom:20px; font-weight: 700">
                
                <img src="https://marketplace.axieinfinity.com/static/image/tab-item.png" alt="">
                Objetos
            </a>
            
        </div>
                
            
    </div>


    {{-- LIST OF ITEMS --}}
   @foreach ($objetos as $objeto)

        <div class="border border-gay-3 rounded hover:shadow hover: border-gay-6">

            <div>
                
                @if ($objeto->planta != null)
                    <div> 
                        <span> #{{ $objeto->planta->catalago_objeto_id }}</span>
                    </div>
                @endif
                

            </div>
        </div>

       <p>{{ $objeto->titulo }}</p>
      


   @endforeach
@endsection